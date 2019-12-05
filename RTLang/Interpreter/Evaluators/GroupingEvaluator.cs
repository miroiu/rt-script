using RTLang.Parser;

namespace RTLang.Interpreter
{
    [ExpressionEvaluator(typeof(GroupingExpression))]
    public class GroupingEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (GroupingExpression)expression;
            var result = Reducer.Reduce(casted.Inner, ctx);
            return result;
        }
    }
}
