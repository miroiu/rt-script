using RTLang.Interpreter;
using RTLang.Parser;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(VariableDeclarationExpression))]
    internal class VariableDeclarationAnalyzer : IExpressionAnalyzer
    {
        public IEnumerable<CompletionItem> GetCompletions(Expression expression, IAnalysisContext context)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (VariableDeclarationExpression)expression;
            var alreadyExists = context.GetSymbols().Any(s => s == casted.Identifier);

            if (alreadyExists)
            {
                yield return new Diagnostic
                {
                    Position = casted.Token.Column,
                    Length = casted.Token.Text.Length,
                    Type = DiagnosticType.Error,
                    Message = $"'{casted.Identifier}' is already defined in the current context."
                };
            }
        }
    }
}
