using RTScript.Expressions;

namespace RTScript.Interpreter.Evaluators
{
    public interface IExpressionEvaluator
    {
        Expression Evaluate(Expression expression, IExecutionContext ctx);
    }
}
