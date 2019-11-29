using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter
{
    [ExpressionEvaluator(typeof(Assignment))]
    public class AssignmentEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (Assignment)expression;
            var result = Reducer.Reduce<ValueExpression>(casted.Initializer, ctx);
            ctx.Assign(casted.Identifier.Name, result.Value);
            return default;
        }
    }
}
