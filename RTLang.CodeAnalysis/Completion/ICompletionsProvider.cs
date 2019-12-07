using RTLang.Parser;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    internal interface ICompletionsProvider
    {
        IEnumerable<Completion> GetCompletions(Expression expression, IAnalysisContext context);
    }
}