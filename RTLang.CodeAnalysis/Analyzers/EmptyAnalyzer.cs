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
        public IEnumerable<Completion> GetCompletions(Expression expression, IAnalysisContext context)
            => context.GetSymbols()
            .Select(s => new Completion
            {
                Type = s.Type,
                Text = s.Name
            });

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
            => Enumerable.Empty<Diagnostic>();

        public Type GetReturnType(Expression expression, IAnalysisContext context)
            => default;
    }
}
