using RTScript.Language.Expressions;
using RTScript.Language.Interop;
using System.Linq;

namespace RTScript.Language.Interpreter.Evaluators
{
    // Not part of a member access expression so it should be treated as global command
    [ExpressionEvaluator(typeof(InvocationExpression))]
    public class InvocationEvaluator : IExpressionEvaluator
    {
        private const string DelegateInvoke = "Invoke";

        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (InvocationExpression)expression;

            var action = ctx.Get(casted.Name);
            var actionType = ctx.GetType(casted.Name);

            if (action != default)
            {
                var methods = TypesCache.GetMethods(actionType).Where(p => p.Descriptor.Name == DelegateInvoke);
                var arguments = casted.Arguments.Items;

                foreach (var method in methods)
                {
                    var parameterTypes = method.Descriptor.Parameters;

                    if (BinaryExpressionEvaluator.TryMatchMethodOverload(ctx, arguments, parameterTypes, out var values))
                    {
                        return new MethodAccessExpression(action, method, values);
                    }
                }

                throw new ExecutionException($"No matching overload found for {casted.Name}'", casted);
            }

            throw new ExecutionException($"'global' is not defined.", casted);
        }
    }
}
