using System;
using System.Collections.Generic;
using System.Linq;
using RTLang.Interpreter;
using RTLang.Parser;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(EmptyExpression))]
    internal class EmptyAnalyzer : IExpressionAnalyzer
    {
        public IEnumerable<CompletionItem> GetCompletions(Expression expression, IAnalysisContext context)
            => context.GetSymbols()
            .Select(s => new CompletionItem
            {
                Type = s.Type,
                Text = s.Name
            });

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
            => new List<Diagnostic>();

        public Type GetReturnType(Expression expression, IAnalysisContext context)
            => default;
    }
}
