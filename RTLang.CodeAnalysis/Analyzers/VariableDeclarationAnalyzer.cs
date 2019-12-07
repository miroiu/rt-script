using RTLang.Interpreter;
using RTLang.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(VariableDeclarationExpression))]
    internal class VariableDeclarationAnalyzer : IExpressionAnalyzer
    {
        // var x = <completions>
        public IEnumerable<CompletionItem> GetCompletions(Expression expression, IAnalysisContext context)
            => context.GetSymbols()
            .Where(s => s.Type == SymbolType.Type || s.Type == SymbolType.Variable)
            .Select(s => new CompletionItem
            {
                Type = s.Type,
                Text = s.Name
            });

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (VariableDeclarationExpression)expression;

            var alreadyExists = context.GetSymbols().Any(s => s.Name == casted.Name);

            if (alreadyExists)
            {
                return new Diagnostic
                {
                    Position = casted.Token.Position,
                    Length = casted.Token.Text.Length,
                    Type = DiagnosticType.Error,
                    Message = $"'{casted.Name}' is already defined in the current context."
                }.ToOneItemArray();
            }

            return new List<Diagnostic>();
        }

        public Type GetReturnType(Expression expression, IAnalysisContext context)
            => default;
    }
}
