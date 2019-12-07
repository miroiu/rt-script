using RTLang.Interpreter;
using RTLang.Parser;
using System;
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
            return context.GetSymbols()
                .Where(s => s.Name != casted.Name && s.Name.StartsWith(casted.Name))
                .Select(w => new CompletionItem
                {
                    Text = w.Name,
                    Type = w.Type
                });
        }

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (IdentifierExpression)expression;
            var exists = context.GetSymbols().Any(s => s.Name == casted.Name);

            if (!exists)
            {
                return new Diagnostic
                {
                    Position = casted.Token.Column,
                    Length = casted.Token.Text.Length,
                    Type = DiagnosticType.Error,
                    Message = $"'{casted.Name}' is not defined in the current context."
                }.ToOneItemArray();
            }

            return new List<Diagnostic>();
        }

        public Type GetReturnType(Expression expression, IAnalysisContext context)
        {
            var casted = (IdentifierExpression)expression;
            return context.GetType(casted.Name);
        }
    }
}