using RTScript.Expressions;
using System;

namespace RTScript.Interpreter.Evaluators
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

            if (firstValue.Type != default)
            {
                var elementType = firstValue.Type;
                var array = Array.CreateInstance(elementType, args.Count);

                for (int i = 0; i < args.Count; i++)
                {
                    var arg = args[i];

                    var value = Reducer.Reduce<ValueExpression>(arg, ctx);

                    var valueType = value.Type;
                    if (!elementType.IsAssignableFrom(valueType))
                    {
                        if (elementType == typeof(char) && valueType == typeof(string) && char.TryParse(value.Value.ToString(), out char c))
                        {
                            array.SetValue(c, i);
                        }
                        else
                        {
                            throw new Exception($"Could not convert type {valueType.ToFriendlyName()} to {elementType.ToFriendlyName()}");
                        }
                    }
                    else
                    {
                        array.SetValue(value.Value, i);
                    }
                }

                return new ValueExpression(array, array.GetType());
            }

            throw new ExecutionException($"First value of an array cannot be null.", expression);
        }
    }
}
