using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    [ExpressionEvaluator(typeof(IndexerAccessExpression))]
    public class IndexerAccessEvaluator : IExpressionEvaluator
    {
        public Expression Evaluate(Expression expression, IExecutionContext ctx)
        {
            var casted = (IndexerAccessExpression)expression;
            return new ValueExpression(casted.Indexer.GetValue(casted.Instance, casted.Index), casted.Indexer.Descriptor.ReturnType);
        }
    }
}
