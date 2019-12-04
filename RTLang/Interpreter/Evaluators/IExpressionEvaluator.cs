using RTLang.Expressions;

namespace RTLang.Interpreter.Evaluators
{
    public interface IExpressionEvaluator
    {
        Expression Evaluate(Expression expression, IExecutionContext ctx);
    }
}
