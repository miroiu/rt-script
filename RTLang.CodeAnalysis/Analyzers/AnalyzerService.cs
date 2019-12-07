using RTLang.Interpreter;
using RTLang.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeAnalysis.Analyzers
{
    internal class AnalyzerService : ILangVisitor<Expression>
    {
        private static readonly IDictionary<Type, IExpressionAnalyzer> Analyzers = typeof(IExpressionAnalyzer).Assembly.GetTypes()
                .Where(x => typeof(IExpressionAnalyzer).IsAssignableFrom(x))
                .SelectMany(x =>
                {
                    return x.GetCustomAttributes(false).Select(y => new
                    {
                        Attribute = (ExpressionEvaluatorAttribute)y,
                        Type = x
                    }).ToList();
                })
                .ToDictionary(x => x.Attribute.ExpressionType, x => Activator.CreateInstance(x.Type) as IExpressionAnalyzer);

        private readonly int _completionPosition;
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        private readonly CompletionPositionFinder _positionFinder = new CompletionPositionFinder();
        private const int NONE = -1;

        public AnalyzerService(IAnalysisContext context, AnalyzerOptions options, int completionPosition = 0)
        {
            Context = context;
            Options = options;
            _completionPosition = completionPosition;
        }

        public IReadOnlyList<Completion> Completions { get; private set; } = new List<Completion>();
        public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;
        public IAnalysisContext Context { get; }
        public AnalyzerOptions Options { get; }

        public void Visit(Expression host)
        {
            if (Analyzers.TryGetValue(host.GetType(), out var analyzer))
            {
                // Happens only once
                if (Completions.Count == 0 && Options.HasFlag(AnalyzerOptions.Completions))
                {
                    bool isAtDesiredPosition = _completionPosition == NONE || _positionFinder.FindPosition(host) == _completionPosition;
                    if (isAtDesiredPosition)
                    {
                        Completions = analyzer.GetCompletions(host, Context).ToList();
                        return;
                    }
                }

                if (Options.HasFlag(AnalyzerOptions.Diagnostics))
                {
                    _diagnostics.AddRange(analyzer.GetDiagnostics(host, Context));
                    return;
                }
            }

            host.Accept(this);
        }

        public static Type GetReturnType(Expression expression, IAnalysisContext context)
        {
            var exprType = expression.GetType();

            if (Analyzers.TryGetValue(exprType, out var analyzer))
            {
                return analyzer.GetReturnType(expression, context);
            }

            return default;
        }

        public static IReadOnlyList<Completion> GetCompletions(Expression expression, IAnalysisContext context)
        {
            var service = new AnalyzerService(context, AnalyzerOptions.Completions, NONE);
            service.Visit(expression);

            return service.Completions;
        }

        // Will stop at the first diagnostic found
        public static IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var service = new AnalyzerService(context, AnalyzerOptions.Diagnostics, NONE);
            service.Visit(expression);

            return service.Diagnostics;
        }
    }
}
