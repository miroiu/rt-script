using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(BinaryExpression))]
    public class BinaryExpressionEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (BinaryExpression)expression;
            var leftExpr = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
            var rightExpr = Reducer.Reduce<ValueExpression>(casted.Right, ctx);

            var result = ctx.Evaluate(leftExpr.Value, casted.OperatorType, rightExpr.Value);

            if (result == null)
            {
                throw new Exception($"Could not determine result type.");
            }

            return new ValueExpression(result, result.GetType());
        }
    }
}
