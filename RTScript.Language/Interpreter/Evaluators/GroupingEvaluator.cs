using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
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
