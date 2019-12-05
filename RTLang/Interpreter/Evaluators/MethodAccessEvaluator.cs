using RTLang.Parser;

namespace RTLang.Interpreter
{
    [ExpressionEvaluator(typeof(MethodAccessExpression))]
    public class MethodAccessEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (MethodAccessExpression)expression;

            if (casted.Instance != null || casted.Method.Descriptor.IsStatic)
            {
                return new ValueExpression(casted.Method.Execute(casted.Instance, (object[])casted.Arguments), casted.Method.Descriptor.ReturnType);
            }

            throw new ExecutionException($"Cannot read method '{casted.Method.Descriptor.Name}' of null.", casted);
        }
    }
}
