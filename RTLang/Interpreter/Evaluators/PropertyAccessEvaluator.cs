using RTLang.Interop;
using RTLang.Parser;

namespace RTLang.Interpreter
{
    [ExpressionEvaluator(typeof(PropertyAccessExpression))]
    public class PropertyAccessEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (PropertyAccessExpression)expression;

            var descriptor = casted.Property.Descriptor;

            if (descriptor.CanRead)
            {
                if (casted.Instance != default || descriptor.IsStatic)
                {
                    return new ValueExpression(casted.Property.GetValue(casted.Instance, casted.Index), descriptor.ReturnType)
                    {
                        Token = casted.Token
                    };
                }

                if (descriptor.DescriptorType == DescriptorType.Indexer)
                {
                    throw new ExecutionException($"Cannot index a null value.", casted);
                }

                throw new ExecutionException($"Cannot read property '{descriptor.Name}' of null.", casted);
            }

            throw new ExecutionException($"Property '{descriptor.Name}' is write-only.", casted);
        }
    }
}
