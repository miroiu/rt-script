using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(InvocationExpression))]
    public class InvocationEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (InvocationExpression)expression;

            //var method = ctx.Get(casted.Name);

            return casted;
        }
    }
}
