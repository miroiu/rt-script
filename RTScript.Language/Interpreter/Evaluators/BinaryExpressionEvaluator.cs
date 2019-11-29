using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(BinaryExpression))]
    public class BinaryExpressionEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (BinaryExpression)expression;
            var left = Reducer.Reduce<ValueExpression>(casted.Left, ctx).Value;
            var right = Reducer.Reduce<ValueExpression>(casted.Right, ctx).Value;

            var result = ctx.Evaluate(left, casted.OperatorType, right);
            return new ValueExpression(result);
        }
    }
}
