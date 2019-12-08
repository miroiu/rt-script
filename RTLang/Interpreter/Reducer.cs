using RTLang.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.Interpreter
{
    internal static class Reducer
    {
        internal static readonly IDictionary<Type, IExpressionEvaluator> Evaluators = typeof(Interpreter).Assembly.GetTypes()
                .Where(x => typeof(IExpressionEvaluator).IsAssignableFrom(x) && x.CustomAttributes.Any())
                .SelectMany(x =>
                {
                    return x.GetCustomAttributes(false).Select(y => new
                    {
                        Attribute = (ExpressionEvaluatorAttribute)y,
                        Type = x
                    }).ToList();
                })
                .ToDictionary(x => x.Attribute.ExpressionType, x => Activator.CreateInstance(x.Type) as IExpressionEvaluator);

        public static Expression Reduce(Expression expression, IExecutionContext ctx, Type resultType = default)
        {
            try
            {
                if (expression == default || expression is ValueExpression)
                {
                    return expression;
                }

                var evaluator = Evaluators[expression.GetType()];
                var expr = evaluator.Evaluate(expression, ctx);

                if (resultType != default && expr.GetType() == resultType)
                {
                    return expr;
                }

                return Reduce(expr, ctx, resultType);
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
