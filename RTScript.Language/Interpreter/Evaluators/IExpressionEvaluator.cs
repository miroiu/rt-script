using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    public interface IExpressionEvaluator
    {
        Expression Evaluate(Expression expression, IExecutionContext ctx);
    }
}
