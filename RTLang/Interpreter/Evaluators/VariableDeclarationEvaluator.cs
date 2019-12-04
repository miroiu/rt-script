using RTLang.Expressions;

namespace RTLang.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(VariableDeclarationExpression))]
    public class VariableDeclarationEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (VariableDeclarationExpression)expression;
            object result = ((ValueExpression)Reducer.Reduce(casted.Initializer, ctx)).Value;
            ctx.Declare(casted.Identifier.Name, result, casted.IsReadOnly);
            return default;
        }
    }
}
