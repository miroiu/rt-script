using RTLang.Interop;
using RTLang.Parser;
using System.Linq;

namespace RTLang.Interpreter
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

                    var property = Reducer.Reduce<PropertyAccessExpression>(casted.Left, ctx, false);
                    if (property != default)
                    {
                        var rightEx = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                        var wrapper = property.Property;
                        var descriptor = wrapper.Descriptor;
                        var returnType = descriptor.ReturnType;
                        var castedValue = rightEx.Value;

                        if (descriptor.CanWrite)
                        {
                            if (TypeHelper.TryChangeType(ref castedValue, returnType))
                            {
                                wrapper.SetValue(property.Instance, castedValue, property.Index);
                                return property;
                            }

                            throw new ExecutionException($"Cannot convert type '{rightEx.Type.ToFriendlyName()}' to '{returnType.ToFriendlyName()}'.", rightEx);
                        }

                        throw new ExecutionException($"Property '{descriptor.Name}' is read-only.", property);
                    }

                    throw new ExecutionException($"Cannot assign a value to '{casted.Left.Token.Text}'.", casted.Left);

                case BinaryOperatorType.AccessMember:
                    if (casted.Right is IdentifierExpression propertyIdentifier)
                    {
                        bool isStatic = casted.Left is IdentifierExpression id && ctx.IsType(id.Name);
                        var instance = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
                        return CreatePropertyAccessor(instance, propertyIdentifier, isStatic);
                    }

                    if (casted.Right is InvocationExpression invocation)
                    {
                        bool isStatic = casted.Left is IdentifierExpression id && ctx.IsType(id.Name);
                        var instance = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
                        return CreateMethodAccessor(ctx, instance, invocation, isStatic);
                    }

                    if (casted.Right is IndexerExpression indexer)
                    {
                        bool isStatic = casted.Left is IdentifierExpression id && ctx.IsType(id.Name);
                        var instance = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
                        return CreatePropertyIndexerAccessor(ctx, indexer, instance, isStatic);
                    }

                    throw new ExecutionException($"'{casted.Right.Token.Text}' is not a valid property name.", casted.Right);

                default:
                    var leftExpr = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
                    var rightExpr = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                    var result = ctx.Evaluate(casted.OperatorType, leftExpr.Value, rightExpr.Value);

                    return new ValueExpression(result, result?.GetType())
                    {
                        Token = casted.Token
                    };
            }
        }

        #region Accessors

        private static Expression CreatePropertyIndexerAccessor(IExecutionContext ctx, IndexerExpression indexer, ValueExpression instance, bool isStatic)
        {
            // This means the value is not a null literal
            if (instance.Type != default)
            {
                // Should be a single indexable property
                var prop = TypeHelper.GetProperties(instance.Type, DescriptorType.Property)
                    .FirstOrDefault(p => p.Descriptor.Name == indexer.PropertyName && p.Descriptor.IsStatic == isStatic);

                if (prop != default)
                {
                    var indexerWrappers = TypeHelper.GetProperties(prop.Descriptor.ReturnType, DescriptorType.Indexer);
                    var index = Reducer.Reduce<ValueExpression>(indexer.Index, ctx);

                    foreach (var wrapper in indexerWrappers)
                    {
                        if (wrapper.Descriptor.ParameterType == index.Type)
                        {
                            return new PropertyAccessExpression(wrapper, prop.GetValue(instance.Value), index.Value)
                            {
                                Token = index.Token
                            };
                        }
                    }

                    throw new ExecutionException($"'{instance.Type.ToFriendlyName()}' does not have an indexable property named '{prop.Descriptor.Name}'.", indexer);
                }

                throw new ExecutionException($"'{instance.Type.ToFriendlyName()}' does not have a {(isStatic ? "static " : string.Empty)}property named '{indexer.PropertyName}'.", indexer);
            }

            throw new ExecutionException($"Cannot index a null value.", instance);
        }

        private static Expression CreateMethodAccessor(IExecutionContext ctx, ValueExpression instance, InvocationExpression method, bool isStatic)
        {
            // This means the value is not a null literal
            if (instance.Type != default)
            {
                var methodWrappers = TypeHelper.GetMethods(instance.Type).Where(p => p.Descriptor.Name == method.MethodName && p.Descriptor.IsStatic == isStatic);

                foreach (var wrapper in methodWrappers)
                {
                    if (InvocationEvaluator.TryFindMethodOverloadWithArguments(ctx, method.Arguments.Items, wrapper.Descriptor.Parameters, out var values))
                    {
                        return new MethodAccessExpression(instance.Value, wrapper, values)
                        {
                            Token = method.Token
                        };
                    }
                }

                throw new ExecutionException($"No matching overload found for {(isStatic ? "static " : string.Empty)}method '{instance.Type.ToFriendlyName()}.{method.MethodName}'", method);
            }

            throw new ExecutionException($"Cannot read method '{method.MethodName}' of null.", instance);
        }

        private static Expression CreatePropertyAccessor(ValueExpression instance, IdentifierExpression property, bool isStatic)
        {
            // This means the value is not a null literal
            if (instance.Type != default)
            {
                // Should be a single property
                var prop = TypeHelper.GetProperties(instance.Type, DescriptorType.Property).FirstOrDefault(p => p.Descriptor.Name == property.Name && p.Descriptor.IsStatic == isStatic);

                if (prop != default)
                {
                    return new PropertyAccessExpression(prop, instance.Value)
                    {
                        Token = property.Token
                    };
                }
                else
                {
                    // Should be a single enum field
                    var enumField = TypeHelper.GetProperties(instance.Type, DescriptorType.Enum).FirstOrDefault(p => p.Descriptor.Name == property.Name);
                    if(enumField != default)
                    {
                        return new PropertyAccessExpression(enumField, instance.Value)
                        {
                            Token = property.Token
                        };
                    }
                }

                throw new ExecutionException($"'{instance.Type.ToFriendlyName()}' does not have a {(isStatic ? "static " : string.Empty)}property named '{property.Name}'.", property);
            }

            throw new ExecutionException($"Cannot read property '{property.Name}' of null.", instance);
        }

        #endregion
    }
}
