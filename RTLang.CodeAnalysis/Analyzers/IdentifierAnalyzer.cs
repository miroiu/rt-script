using RTLang.Interpreter;
using RTLang.Parser;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(IdentifierExpression))]
    internal class IdentifierAnalyzer : IExpressionAnalyzer
    {
        public IEnumerable<CompletionItem> GetCompletions(Expression expression, IAnalysisContext context)
        {
            var casted = (IdentifierExpression)expression;
            var symbols = context.GetSymbols().Where(s => s.StartsWith(casted.Name) && !(s == casted.Name));

            return symbols.Select(w => new CompletionItem
            {
                Text = w
            });
        }

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (IdentifierExpression)expression;
            var symbols = context.GetSymbols().Where(s => s.StartsWith(casted.Name) && !(s == casted.Name));

            if (!symbols.Contains(casted.Name))
            {
                yield return new Diagnostic
                {
                    Position = casted.Token.Column,
                    Length = casted.Token.Text.Length,
                    Type = DiagnosticType.Error,
                    Message = $"'{casted.Name}' is not defined in the current context."
                };
            }
        }
    }
}