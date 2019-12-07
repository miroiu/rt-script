using System;
using System.Collections.Generic;
using RTLang.Parser;

namespace RTLang.CodeAnalysis.Analyzers
{
    internal class UnaryAnalyzer : IExpressionAnalyzer
    {
        public IEnumerable<CompletionItem> GetCompletions(Expression expression, IAnalysisContext context)
        {
            var casted = (UnaryExpression)expression;
            return AnalyzerService.GetCompletions(casted.Operand, context);
        }

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (UnaryExpression)expression;
            return AnalyzerService.GetDiagnostics(casted.Operand, context);
        }

        public Type GetReturnType(Expression expression, IAnalysisContext context)
        {
            var casted = (UnaryExpression)expression;
            var type = AnalyzerService.GetReturnType(casted.Operand, context);

            if (type != default)
            {
                var op = TypeHelper.GetUnaryOperator(casted.OperatorType, type);
                return op?.ReturnType;
            }

            return default;
        }
    }
}
