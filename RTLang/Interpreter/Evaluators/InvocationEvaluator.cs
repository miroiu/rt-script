using RTLang.Expressions;
using RTLang.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.Interpreter.Evaluators
{
    // Not part of a member access expression so it should be treated as global command
    [ExpressionEvaluator(typeof(InvocationExpression))]
    public class InvocationEvaluator : IExpressionEvaluator
    {
        private const string DelegateInvoke = "Invoke";

        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (InvocationExpression)expression;

            var action = ctx.GetValue(casted.MethodName);

            if (action != default)
            {
                var actionType = ctx.GetType(casted.MethodName);
                var methods = TypesCache.GetMethods(actionType).Where(p => p.Descriptor.Name == DelegateInvoke);

                foreach (var method in methods)
                {
                    if (TryFindMethodOverloadWithArguments(ctx, casted.Arguments.Items, method.Descriptor.Parameters, out var values))
                    {
                        return new MethodAccessExpression(action, method, values);
                    }
                }

                throw new ExecutionException($"No matching overload found for '{casted.MethodName}'", casted);
            }

            throw new ExecutionException($"Cannot invoke null delegate '{casted.MethodName}'.", casted);
        }

        internal static bool TryFindMethodOverloadWithArguments(IExecutionContext ctx, IReadOnlyList<Expression> arguments, IReadOnlyList<Type> parameterTypes, out object[] argumentsValues)
        {
            argumentsValues = new object[arguments.Count];

            if (arguments.Count != parameterTypes.Count)
            {
                return false;
            }

            for (var i = 0; i < parameterTypes.Count; i++)
            {
                var arg = Reducer.Reduce<ValueExpression>(arguments[i], ctx);
                var paramType = parameterTypes[i];

                var r = arg.Value;
                if (!TypeHelper.TryChangeType(ref r, paramType))
                {
                    return false;
                }

                argumentsValues[i] = r;
            }

            return true;
        }
    }
}
