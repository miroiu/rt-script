using RTLang.Expressions;
using RTLang.Interop;
using System.Linq;

namespace RTLang.Interpreter.Evaluators
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
                    if (leftProp != null)
                    {
                        var rightEx = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                        leftProp.Property.SetValue(leftProp.Instance, rightEx.Value, leftProp.Index);
                        return leftProp;
                    }

                    throw new ExecutionException($"Expected identifier.", casted.Left);

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
                        var instance = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
                        return CreatePropertyIndexerAccessor(ctx, indexer, instance);
                    }

                    throw new ExecutionException($"Expected identifier.", casted.Left);

                default:
                    var rightExpr = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                    var leftExpr = Reducer.Reduce<ValueExpression>(casted.Left, ctx);
                    var result = ctx.Evaluate(casted.OperatorType, leftExpr.Value, rightExpr.Value);

                    return new ValueExpression(result, result?.GetType());
            }
        }

        #region Accessors

        private static Expression CreatePropertyIndexerAccessor(IExecutionContext ctx, IndexerExpression indexer, ValueExpression instance)
        {
            // This means the value is not a null literal
            if (instance.Type != null)
            {
                // Should be a single indexable property
                var prop = TypesCache.GetProperties(instance.Type).FirstOrDefault(p => !p.Descriptor.IsIndexer && p.Descriptor.Name == indexer.PropertyName);

                if (prop != null)
                {
                    var indexerWrappers = TypesCache.GetProperties(prop.Descriptor.ReturnType).Where(p => p.Descriptor.IsIndexer);
                    var index = Reducer.Reduce<ValueExpression>(indexer.Index, ctx);

                    foreach (var wrapper in indexerWrappers)
                    {
                        if (wrapper.Descriptor.ParameterType == index.Type)
                        {
                            return new PropertyAccessExpression(wrapper, prop.GetValue(instance.Value), index.Value);
                        }
                    }
                }

                throw new ExecutionException($"Object of type '{instance.Type.ToFriendlyName()}' is not indexable.", indexer);
            }

            throw new ExecutionException($"Cannot index a null value.", instance);
        }

        private static Expression CreateMethodAccessor(IExecutionContext ctx, ValueExpression instance, InvocationExpression method, bool isStatic)
        {
            // This means the value is not a null literal
            if (instance.Type != null)
            {
                var methodWrappers = TypesCache.GetMethods(instance.Type).Where(p => p.Descriptor.Name == method.MethodName && p.Descriptor.IsStatic == isStatic);

                foreach (var wrapper in methodWrappers)
                {
                    if (InvocationEvaluator.TryFindMethodOverloadWithArguments(ctx, method.Arguments.Items, wrapper.Descriptor.Parameters, out var values))
                    {
                        return new MethodAccessExpression(instance.Value, wrapper, values);
                    }
                }

                throw new ExecutionException($"No matching overload found for {(isStatic ? "static" : string.Empty)} method '{instance.Type.ToFriendlyName()}.{method.MethodName}'", method);
            }

            throw new ExecutionException($"Cannot read method '{method.MethodName}' of null.", instance);
        }

        private static Expression CreatePropertyAccessor(ValueExpression instance, IdentifierExpression property, bool isStatic)
        {
            // This means the value is not a null literal
            if (instance.Type != null)
            {
                // Should be a single property
                var prop = TypesCache.GetProperties(instance.Type).FirstOrDefault(p => !p.Descriptor.IsIndexer && p.Descriptor.Name == property.Name && p.Descriptor.IsStatic == isStatic);

                if (prop != null)
                {
                    return new PropertyAccessExpression(prop, instance.Value);
                }

                throw new ExecutionException($"Object of type '{instance.Type.ToFriendlyName()}' does not have a {(isStatic ? "static" : string.Empty)} property named '{property.Name}'.", property);
            }

            throw new ExecutionException($"Cannot read property '{property.Name}' of null.", instance);
        }

        #endregion
    }
}
