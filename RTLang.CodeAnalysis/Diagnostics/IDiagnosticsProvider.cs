using RTLang.Parser;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    internal interface IDiagnosticsProvider
    {
        IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context);
    }
}
