using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(MethodAccessExpression))]
    public class MethodAccessEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (MethodAccessExpression)expression;
            return new ValueExpression(casted.Method.Execute(casted.Instance, (object[])casted.Arguments), casted.Method.Descriptor.ReturnType);
        }
    }
}
