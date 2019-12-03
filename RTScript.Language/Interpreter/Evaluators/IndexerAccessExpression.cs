using RTScript.Language.Expressions;
using RTScript.Language.Interop;

namespace RTScript.Language.Interpreter.Evaluators
{
    public class IndexerAccessExpression : Expression
    {
        public IndexerAccessExpression(object instance, object index, IPropertyWrapper indexer)
        {
            Instance = instance;
            Indexer = indexer;
            Index = index;
        }

        public object Instance { get; }
        public IPropertyWrapper Indexer { get; }
        public object Index { get; }
    }
}
