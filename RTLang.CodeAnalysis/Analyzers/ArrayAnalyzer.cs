using System;
using System.Collections.Generic;
using System.Linq;
using RTLang.Interpreter;
using RTLang.Parser;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(ArrayExpression))]
    internal class ArrayAnalyzer : IExpressionAnalyzer
    {
        public IEnumerable<Completion> GetCompletions(Expression expression, IAnalysisContext context)
            => Enumerable.Empty<Completion>();

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (ArrayExpression)expression;

            var args = casted.Arguments.Items;
            if (args.Count > 0)
            {
                List<Diagnostic> diagnostics = new List<Diagnostic>();
                foreach (var arg in args)
                {
                    diagnostics.AddRange(AnalyzerService.GetDiagnostics(arg, context));
                }
                return diagnostics;
            }

            return new Diagnostic
            {
                Type = DiagnosticType.Error,
                Position = casted.Token.Position,
                Length = casted.Token.Text.Length,
                Message = "Empty arrays are not allowed."
            }.ToOneItemArray();
        }

        public Type GetReturnType(Expression expression, IAnalysisContext context)
        {
            var casted = (ArrayExpression)expression;

            var args = casted.Arguments.Items;
            if (args.Count > 0)
            {
                return AnalyzerService.GetReturnType(args[0], context)
                    ?.MakeArrayType();
            }

            return default;
        }
    }
}
