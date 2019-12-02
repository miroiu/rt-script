using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(PropertyAccessExpression))]
    public class PropertyAccessEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (PropertyAccessExpression)expression;
            return new ValueExpression(casted.Property.GetValue(casted.Instance), casted.Property.Descriptor.PropertyType);
        }
    }
}
