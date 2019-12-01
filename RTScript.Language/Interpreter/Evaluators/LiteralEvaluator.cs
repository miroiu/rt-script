using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(LiteralExpression))]
    // Late evaluation of literal value
    public class LiteralEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (LiteralExpression)expression;
            var result = ctx.Evaluate(casted.Type, casted.Value);

            if (result == null)
            {
                throw new Exception("Could not determine result type.");
            }

            return new ValueExpression(result, result.GetType());
        }
    }
}
