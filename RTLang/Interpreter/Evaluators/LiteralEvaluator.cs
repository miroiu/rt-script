using RTLang.Expressions;

namespace RTLang.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(LiteralExpression))]
    // Late evaluation of literal value
    public class LiteralEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (LiteralExpression)expression;
            var result = ctx.Evaluate(casted.Type, casted.Value);

            // Allow nulls for null literal
            return new ValueExpression(result, result?.GetType());
        }
    }
}
