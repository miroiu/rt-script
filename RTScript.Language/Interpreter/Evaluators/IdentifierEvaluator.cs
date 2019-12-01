using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(IdentifierExpression))]
    public class IdentifierEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (IdentifierExpression)expression;
            var result = ctx.Get(casted.Name);
            var type = ctx.GetType(casted.Name);
            return new ValueExpression(result, type);
        }
    }
}
