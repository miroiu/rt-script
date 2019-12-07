using System;
using System.Collections.Generic;
using System.Linq;
using RTLang.Interpreter;
using RTLang.Parser;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(LiteralExpression))]
    internal class LiteralAnalyzer : IExpressionAnalyzer
    {
        public IEnumerable<Completion> GetCompletions(Expression expression, IAnalysisContext context)
            => Enumerable.Empty<Completion>();

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
            => Enumerable.Empty<Diagnostic>();

        public Type GetReturnType(Expression expression, IAnalysisContext context)
        {
            var casted = (LiteralExpression)expression;
            return context.GetLiteralType(casted.Type, casted.Value);
        }
    }
}
