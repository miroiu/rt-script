using RTLang.Parser;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    internal interface ICompletionsProvider
    {
        IEnumerable<CompletionItem> GetCompletions(Expression expression, IAnalysisContext context);
    }
}