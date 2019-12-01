using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(UnaryExpression))]
    public class UnaryEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (UnaryExpression)expression;
            var value = Reducer.Reduce<ValueExpression>(casted.Operand, ctx).Value;

            switch (casted.OperatorType)
            {
                case UnaryOperatorType.Print:
                    ctx.Print(value);
                    return default;

                default:
                    var result = ctx.Evaluate(casted.OperatorType, value);

                    if (result == null)
                    {
                        throw new Exception("Could not determine result type.");
                    }

                    return new ValueExpression(result, result.GetType());
            }
        }
    }
}
