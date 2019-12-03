using RTScript.Language.Expressions;
using RTScript.Language.Interop;
using System.Linq;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(IndexerExpression))]
    public class IndexerEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (IndexerExpression)expression;
            var instance = ctx.Get(casted.Name);

            if (instance != null)
            {
                var instanceType = ctx.GetType(casted.Name);
                var indexers = TypesCache.GetProperties(instanceType).Where(p => p.Descriptor.IsIndexer);

                var indexValue = Reducer.Reduce<ValueExpression>(casted.Index, ctx);

                if (indexValue.Value != default)
                {
                    foreach (var indexer in indexers)
                    {
                        if (indexer.Descriptor.ParameterType == indexValue.Type)
                        {
                            return new IndexerAccessExpression(instance, indexValue.Value, indexer);
                        }
                    }

                    throw new ExecutionException($"Object of type '{instanceType.ToFriendlyName()}' does not have an index taking a '{indexValue.Type.ToFriendlyName()}' parameter.", casted);
                }

                throw new ExecutionException($"Index was null.", casted.Index);
            }

            throw new ExecutionException($"Cannot apply indexing to null value.", casted);
        }
    }
}
