using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter.Evaluators
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
                    // TODO: Try to convert value to element type (e.g. an array of ints will not work because the default number type is double, nor char array)
                    var valueType = value.Type;
                    if (!elementType.IsAssignableFrom(valueType))
                    {
                        throw new Exception($"Could not convert type {valueType.ToFriendlyName()} to {elementType.ToFriendlyName()}");
                    }

                    array.SetValue(value.Value, i);
                }

                return new ValueExpression(array, array.GetType());
            }

            throw new ExecutionException($"First value of an array cannot be null.", expression);
        }
    }
}
