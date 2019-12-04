using RTLang.Expressions;

namespace RTLang.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(LiteralExpression))]
    public class LiteralEvaluator : IExpressionEvaluator
    {
        // Late evaluation of literal value
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (LiteralExpression)expression;
            var result = ctx.Evaluate(casted.Type, casted.Value);

            // The null literal doesn't have a type
            return new ValueExpression(result, result?.GetType());
        }
    }
}
