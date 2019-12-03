using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(MethodCallExpression))]
    public class MethodCallEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (MethodCallExpression)expression;
            return new ValueExpression(casted.Method.Execute(casted.Instance, (object[])casted.Arguments), casted.Method.Descriptor.ReturnType);
        }
    }
}
