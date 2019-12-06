using RTLang.Interpreter;
using RTLang.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeAnalysis.Analyzers
{
    internal class AnalyzerService : ILangVisitor<Expression>
    {
        internal static readonly IDictionary<Type, IExpressionAnalyzer> Analyzers = typeof(IExpressionAnalyzer).Assembly.GetTypes()
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
        private readonly List<Diagnostic> _diagnostics = Collections<Diagnostic>.Empty;

        public AnalyzerService(IAnalysisContext context, AnalyzerOptions options, int completionPosition = 0)
        {
            Context = context;
            Options = options;
            _completionPosition = completionPosition;
        }

        public IReadOnlyList<CompletionItem> Completions { get; private set; } = Collections<CompletionItem>.Empty;
        public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;
        public IAnalysisContext Context { get; }
        public AnalyzerOptions Options { get; }

        public void Visit(Expression host)
        {
            if (Analyzers.TryGetValue(host.GetType(), out var analyzer))
            {
                // Happens only once
                if (Options.HasFlag(AnalyzerOptions.Completions) && (host.Token.Position + host.Token.Text.Length == _completionPosition))
                {
                    Completions = analyzer.GetCompletions(host, Context).ToList();
                    return;
                }

                if (Options.HasFlag(AnalyzerOptions.Diagnostics))
                {
                    _diagnostics.AddRange(analyzer.GetDiagnostics(host, Context));
                }
            }

            host.Accept(this);
        }
    }
}
