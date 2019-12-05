using RTLang.Parser;

namespace RTLang.Interpreter
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
                    return new ValueExpression(result, result?.GetType());
            }
        }
    }
}
