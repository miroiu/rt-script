using RTLang.Expressions;
using RTLang.Interop;
using System.Linq;

namespace RTLang.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(IndexerExpression))]
    public class IndexerEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (IndexerExpression)expression;
            var instance = ctx.GetValue(casted.IdentifierName);

            if (instance != null)
            {
                var instanceType = ctx.GetType(casted.IdentifierName);
                var indexers = TypesCache.GetProperties(instanceType).Where(p => p.Descriptor.IsIndexer);

                var indexValue = Reducer.Reduce<ValueExpression>(casted.Index, ctx);

                if (indexValue.Value != default)
                {
                    foreach (var indexer in indexers)
                    {
                        if (indexer.Descriptor.ParameterType == indexValue.Type)
                        {
                            return new PropertyAccessExpression(indexer, instance, indexValue.Value);
                        }
                    }

                    throw new ExecutionException($"Object of type '{instanceType.ToFriendlyName()}' does not have an index taking a '{indexValue.Type.ToFriendlyName()}' parameter.", casted);
                }

                throw new ExecutionException($"Index is null.", casted.Index);
            }

            throw new ExecutionException($"Cannot index a null value.", casted);
        }
    }
}
