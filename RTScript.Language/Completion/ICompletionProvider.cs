using RTScript.Language.Expressions;
using System.Collections.Generic;

namespace RTScript.Language.Completion
{
    public interface ICompletionProvider
    {
        IEnumerable<string> GetCompletion(Expression expression, ICompletionContext ctx);
    }
}