using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(BinaryExpression))]
    public class BinaryExpressionEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (BinaryExpression)expression;

            switch (casted.OperatorType)
            {
                case BinaryOperatorType.Assign:
                    if (casted.Left is IdentifierExpression identifier)
                    {
                        var rightEx = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                        ctx.Assign(identifier.Name, rightEx.Value);
                        return rightEx;
                    }

                    throw new ExecutionException($"Expected identifier.", casted.Left);

                default:
                    var rightExpr = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                    var leftExpr = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
                    var result = ctx.Evaluate(leftExpr.Value, casted.OperatorType, rightExpr.Value);

                    if (result == null)
                    {
                        throw new ExecutionException($"Could not determine result type.", expression);
                    }

                    return new ValueExpression(result, result.GetType());
            }
        }
    }
}
