using RTScript.Language.Expressions;
using RTScript.Language.Interpreter;
using RTScript.Language.Lexer;
using RTScript.Language.Parser;
using System.Collections.Generic;
using System.Linq;

namespace RTScript.Language.Completion
{
    public class RTScriptCompletionService
    {
        private readonly ICompletionContext _context;

        public RTScriptCompletionService(IExecutionContext context)
        {
            _context = new CompletionContext(context);
        }

        public IReadOnlyList<string> GetSuggestions(string input)
        {
            var source = new SourceText(input);
            var lexer = new RTScriptLexer(source);
            var parser = new RTScriptParser(lexer);


            return Enumerable.Empty<string>().ToList();
        }
    }
}
