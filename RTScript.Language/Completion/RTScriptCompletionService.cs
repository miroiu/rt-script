using RTScript.Language.Interpreter;
using RTScript.Language.Lexer;
using RTScript.Language.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTScript.Language.Completion
{
    public class RTScriptCompletionService
    {
        private readonly ICompletionContext _context;

        public static readonly IDictionary<Type, ICompletionProvider> Providers = typeof(RTScriptCompletionService).Assembly.GetTypes()
                .Where(x => typeof(ICompletionProvider).IsAssignableFrom(x) && x.CustomAttributes.Any())
                .SelectMany(x =>
                {
                    return x.GetCustomAttributes(false).Select(y => new
                    {
                        Attribute = (CompletionProviderAttribute)y,
                        Type = x
                    }).ToList();
                })
                .ToDictionary(x => x.Attribute.ExpressionType, x => Activator.CreateInstance(x.Type) as ICompletionProvider);


        public RTScriptCompletionService(IExecutionContext context)
        {
            _context = new CompletionContext(context);
        }

        public IReadOnlyList<string> GetSuggestions(string input)
        {
            var source = new SourceText(input);
            var lexer = new RTScriptLexer(source);
            var parser = new RTScriptParser(lexer);

            // TODO: No idea

            return Enumerable.Empty<string>().ToList();
        }
    }
}
