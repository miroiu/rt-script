using RTLang.Expressions;

namespace RTLang.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(EmptyExpression))]
    public class EmptyEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
            => default;
    }
}
