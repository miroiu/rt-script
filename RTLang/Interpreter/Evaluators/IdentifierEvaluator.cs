using RTLang.Parser;

namespace RTLang.Interpreter
{
    [ExpressionEvaluator(typeof(IdentifierExpression))]
    public class IdentifierEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (IdentifierExpression)expression;
            var result = ctx.GetValue(casted.Name);
            var type = ctx.GetType(casted.Name);

            return new ValueExpression(result, type)
            {
                Token = casted.Token
            };
        }
    }
}
