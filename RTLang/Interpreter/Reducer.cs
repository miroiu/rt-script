using RTScript.Expressions;
using RTScript.Interpreter.Evaluators;
using System;

namespace RTScript.Interpreter
{
    public static class Reducer
    {
        public static Expression Reduce(Expression expression, IExecutionContext ctx, Type resultType = default)
        {
            try
            {
                if (expression == null || expression is ValueExpression)
                {
                    return expression;
                }

                var evaluator = RTScriptInterpreter.Evaluators[expression.GetType()];
                var expr = evaluator.Evaluate(expression, ctx);

                if (resultType != default && expr.GetType() == resultType)
                {
                    return expr;
                }

                return Reduce(expr, ctx);
            }
            catch (ExecutionException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ExecutionException(e, expression);
            }
        }

        public static T Reduce<T>(Expression expression, IExecutionContext ctx, bool throwIfUnexpected = true) where T : Expression
        {
            if (expression is T result)
            {
                return result;
            }

            if (Reduce(expression, ctx, typeof(T)) is T reduced)
            {
                return reduced;
            }

            return throwIfUnexpected ? throw new ExecutionException($"Expected expression of type '{typeof(T).Name}'.", expression) : default(T);
        }
    }
}
