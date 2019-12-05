using RTLang.Parser;

namespace RTLang.Interpreter
{
    public interface IExpressionEvaluator
    {
        Expression Evaluate(Expression expression, IExecutionContext ctx);
    }
}
