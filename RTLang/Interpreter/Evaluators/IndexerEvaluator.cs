using RTLang.Interop;
using RTLang.Parser;
using System.Linq;

namespace RTLang.Interpreter
{
    [ExpressionEvaluator(typeof(IndexerExpression))]
    public class IndexerEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (IndexerExpression)expression;
            var instance = ctx.GetValue(casted.PropertyName);

            if (instance != default)
            {
                var instanceType = ctx.GetType(casted.PropertyName);
                var indexers = TypeHelper.GetProperties(instanceType, DescriptorType.Indexer);

                var indexValue = Reducer.Reduce<ValueExpression>(casted.Index, ctx);

                if (indexValue.Value != default)
                {
                    foreach (var indexer in indexers)
                    {
                        if (indexer.Descriptor.ParameterType == indexValue.Type)
                        {
                            return new PropertyAccessExpression(indexer, instance, indexValue.Value)
                            {
                                Token = casted.Token
                            };
                        }
                    }

                    throw new ExecutionException($"'{instanceType.ToFriendlyName()}' does not have an index taking a '{indexValue.Type.ToFriendlyName()}' parameter.", casted);
                }

                throw new ExecutionException($"Index is null.", casted.Index);
            }

            throw new ExecutionException($"Cannot index a null value.", casted);
        }
    }
}
