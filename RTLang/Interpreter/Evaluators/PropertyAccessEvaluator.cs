using RTScript.Expressions;

namespace RTScript.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(PropertyAccessExpression))]
    public class PropertyAccessEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (PropertyAccessExpression)expression;

            var descriptor = casted.Property.Descriptor;
            if (casted.Instance != null || descriptor.IsStatic)
            {
                return new ValueExpression(casted.Property.GetValue(casted.Instance, casted.Index), descriptor.ReturnType);
            }

            if (descriptor.IsIndexer)
            {
                throw new ExecutionException($"Cannot index a null value.", casted);
            }

            throw new ExecutionException($"Cannot read property '{descriptor.Name}' of null.", casted);
        }
    }
}
