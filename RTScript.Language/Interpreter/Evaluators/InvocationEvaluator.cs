using RTScript.Language.Expressions;
using RTScript.Language.Interop;
using System.Linq;

namespace RTScript.Language.Interpreter.Evaluators
{
    // Not part of a member access expression so it should be treated as global
    [ExpressionEvaluator(typeof(InvocationExpression))]
    public class InvocationEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (InvocationExpression)expression;

            var global = ctx.Get("global");
            var globalType = ctx.GetType("global");

            if (global != null)
            {
                var methods = TypesCache.GetMethods(globalType).Where(p => p.Descriptor.Name == casted.Name);
                var arguments = casted.Arguments.Items;

                foreach (var method in methods)
                {
                    var parameterTypes = method.Descriptor.Parameters;

                    if (BinaryExpressionEvaluator.TryMatchMethodOverload(ctx, arguments, parameterTypes, out var values))
                    {
                        return new MethodAccessExpression(global, method, values);
                    }
                }

                throw new ExecutionException($"No matching overload found for {casted.Name}'", casted);
            }

            throw new ExecutionException($"'global' is not defined.", casted);
        }
    }
}
