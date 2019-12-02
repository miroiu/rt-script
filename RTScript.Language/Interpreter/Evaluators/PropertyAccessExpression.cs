using RTScript.Language.Expressions;
using RTScript.Language.Interop;

namespace RTScript.Language.Interpreter.Evaluators
{
    // Used only by the evaluators.
    public class PropertyAccessExpression : Expression
    {
        public PropertyAccessExpression(object instance, IPropertyWrapper property)
        {
            Instance = instance;
            Property = property;
        }

        public object Instance { get; }
        public IPropertyWrapper Property { get; }
    }
}
