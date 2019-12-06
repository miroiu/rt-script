using RTLang.CodeAnalysis.Analyzers;
using RTLang.CodeAnalysis.Syntax;
using RTLang.Lexer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RTLang.CodeAnalysis
{
    public sealed class RTLangService
    {
        private readonly IAnalysisContext _context;

        private RTLangService(IAnalysisContext context)
            => _context = context;

        public static RTLangService Create(IExecutionContext context)
            => new RTLangService(new AnalysisContext(context));

        public async Task<IReadOnlyList<CompletionItem>> GetCompletionsAsync(string code, int position)
            => await Task.Run(() => GetCompletions(code, position));

        public IReadOnlyList<CompletionItem> GetCompletions(string code, int position)
        {
            if (position <= code.Length)
            {
                using (var text = new SourceText(code.Substring(0, position)))
                {
                    var lexer = new Lexer.Lexer(text);
                    var parser = new SyntaxParser(lexer);

                    try
                    {
                        var analyzer = new AnalyzerService(_context, AnalyzerOptions.Completions, position);

                        while (parser.HasNext)
                        {
                            var expr = parser.Next();
                            analyzer.Visit(expr);
                        }

                        return analyzer.Completions;
                    }
                    catch
                    {
                        return Collections<CompletionItem>.Empty;
                    }
                }
            }

            return Collections<CompletionItem>.Empty;
        }

        public IReadOnlyList<Diagnostic> GetDiagnostics(string code)
        {
            using (var text = new SourceText(code))
            {
                var lexer = new Lexer.Lexer(text);
                var parser = new SyntaxParser(lexer);

                try
                {
                    var analyzer = new AnalyzerService(_context, AnalyzerOptions.Diagnostics);

                    while (parser.HasNext)
                    {
                        var expr = parser.Next();
                        analyzer.Visit(expr);
                    }

                    return analyzer.Diagnostics;
                }
                catch
                {
                    return Collections<Diagnostic>.Empty;
                }
            }
        }
    }
}
