using System.Collections.Generic;
using System.Linq;
using RTScript.Language.Expressions;
using RTScript.Language.Interop;

namespace RTScript.Language.Completion
{
    [CompletionProvider(typeof(BinaryExpression))]
    public class MemberAccessCompletion : ICompletionProvider
    {
        public IEnumerable<string> GetCompletion(Expression expression, ICompletionContext ctx)
        {
            var casted = (BinaryExpression)expression;

            if (casted.OperatorType == BinaryOperatorType.AccessMember)
            {
                if (casted.Right is IdentifierExpression propertyIdentifier)
                {
                    // TODO: Will only work for the root object
                    var type = ctx.GetSymbolType(propertyIdentifier.Name);

                    if (type != null)
                    {
                        // Should be a single property
                        return TypesCache.GetProperties(type).Where(p => !p.Descriptor.IsIndexer && p.Descriptor.Name.StartsWith(propertyIdentifier.Name)).Select(p => p.Descriptor.Name);
                    }
                }
            }

            return Enumerable.Empty<string>();
        }
    }
}
