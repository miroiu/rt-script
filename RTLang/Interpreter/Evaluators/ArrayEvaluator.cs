using RTLang.Parser;
using System;

namespace RTLang.Interpreter
{
    [ExpressionEvaluator(typeof(ArrayExpression))]
    public class ArrayEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (ArrayExpression)expression;

            var args = casted.Arguments.Items;
            var firstArg = args[0];
            var firstValue = Reducer.Reduce<ValueExpression>(firstArg, ctx);

            // This means it's not a null literal
            if (firstValue.Type != default)
            {
                var elementType = firstValue.Type;
                var array = Array.CreateInstance(elementType, args.Count);

                for (int i = 0; i < args.Count; i++)
                {
                    var arg = args[i];
                    var valueExpr = Reducer.Reduce<ValueExpression>(arg, ctx);
                    var value = valueExpr.Value;

                    if (TypeHelper.TryChangeType(ref value, elementType))
                    {
                        array.SetValue(value, i);
                    }
                    else
                    {
                        throw new ExecutionException($"All array values must be of the same type: '{elementType.ToFriendlyName()}'.", arg);
                    }
                }

                return new ValueExpression(array, array.GetType())
                {
                    Token = casted.Token
                };
            }

            throw new ExecutionException($"Could not infer element type because the first element was null.", expression);
        }
    }
}
