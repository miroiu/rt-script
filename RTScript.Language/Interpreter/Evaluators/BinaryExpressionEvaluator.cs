using RTScript.Language.Expressions;
using RTScript.Language.Interop;
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

                    var leftExp = Reducer.Reduce<PropertyAccessExpression>(casted.Left, ctx, false);

                    if (leftExp is PropertyAccessExpression propExpr)
                    {
                        var rightEx = Reducer.Reduce<ValueExpression>(casted.Right, ctx);
                        propExpr.Property.SetValue(propExpr.Instance, rightEx.Value);
                        return propExpr;
                    }

                    throw new ExecutionException($"Expected identifier.", casted.Left);

                case BinaryOperatorType.AccessMember:
                    if (casted.Right is IdentifierExpression propertyIdentifier)
                    {
                        var leftEx = Reducer.Reduce<ValueExpression>(casted.Left, ctx);

                        if (leftEx.Value != null)
                        {
                            // Should be a single property
                            var prop = TypesCache.GetProperties(leftEx.Type).FirstOrDefault(p => p.Descriptor.Name == propertyIdentifier.Name);

                            if (prop != null)
                            {
                                return new PropertyAccessExpression(leftEx.Value, prop);
                            }

                            throw new ExecutionException($"Object of type '{leftEx.Type.ToFriendlyName()}' does not have a property named '{propertyIdentifier.Name}'.", casted.Right);
                        }

                        throw new ExecutionException($"Cannot read property '{propertyIdentifier.Name}' of null.", leftEx);
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
    }
}
