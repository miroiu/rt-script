using RTLang.Expressions;
using RTLang.Interop;

namespace RTLang.Interpreter.Evaluators
{
    // Used only by the evaluators.
    public class PropertyAccessExpression : Expression
    {
        public PropertyAccessExpression(IPropertyWrapper property, object instance, object index = default)
        {
            Instance = instance;
            Index = index;
            Property = property;
        }

        public object Instance { get; }
        public object Index { get; }
        public IPropertyWrapper Property { get; }
    }
}
