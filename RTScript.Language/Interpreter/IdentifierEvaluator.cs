using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter
{
    [ExpressionEvaluator(typeof(Identifier))]
    public class IdentifierEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (Identifier)expression;
            var result = ctx.Get(casted.Name);
            return new ValueExpression(result);
        }
    }
}
