using RTLang.Parser;
using System;

namespace RTLang.CodeAnalysis.Analyzers
{
    internal interface ITypeAnalyzer
    {
        Type GetReturnType(Expression expression, IAnalysisContext context);
    }
}
