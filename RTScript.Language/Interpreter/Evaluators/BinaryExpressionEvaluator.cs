using RTScript.Language.Expressions;
using RTScript.Language.Interop;
using System.Collections.Generic;
using System.Linq;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(BinaryExpression))]
    public class BinaryExpressionEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (BinaryExpression)expression;

            switch (casted.OperatorType)
            {
                case BinaryOperatorType.Assign:
                    if (casted.Left is IdentifierExpression identifier)
                    {
                        var rightEx = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                        ctx.Assign(identifier.Name, rightEx.Value);
                        return rightEx;
                    }

                    var leftProp = Reducer.Reduce<PropertyAccessExpression>(casted.Left, ctx, false);

                    if (leftProp is PropertyAccessExpression propExpr)
                    {
                        var rightEx = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                        propExpr.Property.SetValue(propExpr.Instance, rightEx.Value);
                        return propExpr;
                    }

                    var leftIndexer = Reducer.Reduce<IndexerAccessExpression>(casted.Left, ctx, false);

                    if (leftIndexer is IndexerAccessExpression indexerExpr)
                    {
                        var rightEx = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                        indexerExpr.Indexer.SetValue(indexerExpr.Instance, rightEx.Value, leftIndexer.Index);
                        return indexerExpr;
                    }

                    throw new ExecutionException($"Expected identifier.", casted.Left);

                case BinaryOperatorType.AccessMember:
                    if (casted.Right is IdentifierExpression propertyIdentifier)
                    {
                        var variable = Reducer.Reduce<ValueExpression>(casted.Left, ctx);

                        if (variable.Value != null)
                        {
                            // Should be a single property
                            var prop = TypesCache.GetProperties(variable.Type).FirstOrDefault(p => !p.Descriptor.IsIndexer && p.Descriptor.Name == propertyIdentifier.Name);

                            if (prop != null)
                            {
                                return new PropertyAccessExpression(variable.Value, prop);
                            }

                            throw new ExecutionException($"Object of type '{variable.Type.ToFriendlyName()}' does not have a property named '{propertyIdentifier.Name}'.", casted.Right);
                        }

                        throw new ExecutionException($"Cannot read property '{propertyIdentifier.Name}' of null.", variable);
                    }

                    if (casted.Right is InvocationExpression invocation)
                    {
                        var variable = Reducer.Reduce<ValueExpression>(casted.Left, ctx);

                        if (variable.Value != null)
                        {
                            var methods = TypesCache.GetMethods(variable.Type).Where(p => p.Descriptor.Name == invocation.Name);
                            var arguments = invocation.Arguments.Items;

                            foreach (var method in methods)
                            {
                                var parameterTypes = method.Descriptor.Parameters;

                                if (TryMatchMethodOverload(ctx, arguments, parameterTypes, out var values))
                                {
                                    return new MethodAccessExpression(variable.Value, method, values);
                                }
                            }

                            throw new ExecutionException($"No matching overload found for '{variable.Type.ToFriendlyName()}.{invocation.Name}'", casted.Right);
                        }

                        throw new ExecutionException($"Cannot read method '{invocation.Name}' of null.", variable);
                    }

                    if (casted.Right is IndexerExpression indexer)
                    {
                        var variable = Reducer.Reduce<ValueExpression>(casted.Left, ctx);

                        if (variable.Value != null)
                        {
                            // Should be a single property
                            var prop = TypesCache.GetProperties(variable.Type).FirstOrDefault(p => !p.Descriptor.IsIndexer && p.Descriptor.Name == indexer.Name);

                            if (prop != null)
                            {
                                var indexerWrappers = TypesCache.GetProperties(prop.Descriptor.ReturnType).Where(p => p.Descriptor.IsIndexer);
                                var index = Reducer.Reduce<ValueExpression>(indexer.Index, ctx);

                                foreach (var wrapper in indexerWrappers)
                                {
                                    if (wrapper.Descriptor.ParameterType == index.Type)
                                    {
                                        return new IndexerAccessExpression(prop.GetValue(variable.Value), index.Value, wrapper);
                                    }
                                }
                            }

                            throw new ExecutionException($"Object of type '{variable.Type.ToFriendlyName()}' does not have a property named '{indexer.Name}'.", casted.Right);
                        }

                        throw new ExecutionException($"Cannot read indexer of null.", variable);
                    }

                    throw new ExecutionException($"Expected identifier.", casted.Left);

                default:
                    var rightExpr = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                    var leftExpr = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
                    var result = ctx.Evaluate(leftExpr.Value, casted.OperatorType, rightExpr.Value);

                    if (result == null)
                    {
                        throw new ExecutionException($"Could not determine result type.", expression);
                    }

                    return new ValueExpression(result, result.GetType());
            }
        }

        private static bool TryMatchMethodOverload(IExecutionContext ctx, IReadOnlyList<Expression> arguments, IReadOnlyList<System.Type> parameterTypes, out object[] argumentsValues)
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

                // TODO: Try cast if not matching? (should cast only chars/strings/numbers?)
                if (arg.Type != paramType)
                {
                    return false;
                }

                argumentsValues[i] = arg.Value;
            }

            return true;
        }
    }
}
