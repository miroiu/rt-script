using RTScript.Language.Expressions;
using RTScript.Language.Interpreter.Evaluators;
using System;

namespace RTScript.Language.Interpreter
{
    public static class Reducer
    {
        public static Expression Reduce(Expression expression, IExecutionContext ctx)
        {
            try
            {
                if (expression == null || expression is ValueExpression)
                {
                    return expression;
                }

                var evaluator = RTScriptInterpreter.Evaluators[expression.GetType()];
                var expr = evaluator.Evaluate(expression, ctx);

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

        public static T Reduce<T>(Expression expression, IExecutionContext ctx) where T : Expression
        {
            if(Reduce(expression, ctx) is T result)
            {
                return result;
            }

            throw new ExecutionException($"Expected expression of type {typeof(T).Name}.", expression);
        }
    }
}
