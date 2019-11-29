using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(VariableDeclaration))]
    public class VariableDeclarationEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (VariableDeclaration)expression;
            object result = ((ValueExpression)Reducer.Reduce(casted.Initializer, ctx)).Value;
            ctx.Declare(casted.Identifier.Name, result);
            return default;
        }
    }
}
