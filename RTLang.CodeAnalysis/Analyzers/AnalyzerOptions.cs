using System;

namespace RTLang.CodeAnalysis.Analyzers
{
    [Flags]
    internal enum AnalyzerOptions
    {
        Completions = 1,
        Diagnostics = 2,
        All = Completions | Diagnostics
    }
}
