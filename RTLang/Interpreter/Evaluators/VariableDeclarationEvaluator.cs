using RTLang.Parser;

namespace RTLang.Interpreter
{
    [ExpressionEvaluator(typeof(VariableDeclarationExpression))]
    public class VariableDeclarationEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (VariableDeclarationExpression)expression;
            object result = ((ValueExpression)Reducer.Reduce(casted.Initializer, ctx)).Value;
            ctx.Declare(casted.Name, result, casted.IsReadOnly);
            return default;
        }
    }
}
