using RTLang.CodeCompletion.Evaluator;
using RTLang.Lexer;
using RTLang.Parser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RTLang.CodeCompletion
{
    public sealed class CompletionService
    {
        private readonly ICompletionContext _context;

        private CompletionService(ICompletionContext context)
            => _context = context;

        public static CompletionService Create(IExecutionContext context)
            => new CompletionService(new CompletionContext(context));

        public async Task<CompletionServiceResult> GetCompletionsAsync(string code, int position)
            => await Task.Run(() => GetCompletions(code, position));

        public CompletionServiceResult GetCompletions(string code, int position)
        {
            if (position <= code.Length)
            {
                using (var text = new SourceText(code.Substring(0, position)))
                {
                    var lexer = new Lexer.Lexer(text);
                    var parser = new CompletionParser(lexer);

                    try
                    {
                        return GetCompletions(parser, position);
                    }
                    catch
                    {
                        return CompletionServiceResult.Empty;
                    }
                }
            }

            return CompletionServiceResult.Empty;
        }

        #region Privates

        private CompletionServiceResult GetCompletions(IExpressionProvider provider, int position)
        {
            while (provider.HasNext)
            {
                var expr = provider.Next();
                var bag = new EvaluationBag(_context, position);
                var reducer = new Reducer(bag);

                reducer.Visit(expr);

                return new CompletionServiceResult(bag.Completions, bag.Errors);
            }

            return CompletionServiceResult.Empty;
        }

        #endregion
    }
}
