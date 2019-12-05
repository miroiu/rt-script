using RTLang.Parser;

namespace RTLang.Interpreter
{
    [ExpressionEvaluator(typeof(EmptyExpression))]
    public class EmptyEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
            => default;
    }
}
