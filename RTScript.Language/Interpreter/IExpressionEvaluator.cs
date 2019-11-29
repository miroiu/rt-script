using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter
{
    public interface IExpressionEvaluator
    {
        Expression Evaluate(Expression expression, IExecutionContext ctx);
    }
}
