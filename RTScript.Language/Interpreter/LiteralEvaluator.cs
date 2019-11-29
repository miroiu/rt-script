using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter
{
    [ExpressionEvaluator(typeof(Literal))]
    // Late evaluation of literal value
    public class LiteralEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (Literal)expression;
            var result = ctx.Evaluate(casted.Type, casted.Value);
            return new ValueExpression(result);
        }
    }
}
