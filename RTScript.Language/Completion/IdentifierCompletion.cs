using System.Collections.Generic;
using RTScript.Language.Expressions;

namespace RTScript.Language.Completion
{
    [CompletionProvider(typeof(IdentifierExpression))]
    public class IdentifierCompletion : ICompletionProvider
    {
        public IEnumerable<string> GetCompletion(Expression expression, ICompletionContext ctx)
        {
            var casted = (IdentifierExpression)expression;
            return ctx.GetSymbolCompletion(casted.Name);
        }
    }
}
