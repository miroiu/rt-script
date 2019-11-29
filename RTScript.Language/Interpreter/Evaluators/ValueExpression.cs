using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Evaluators
{
    // Used only by the evaluators.
    // Leaf node in the expression tree
    public class ValueExpression : Expression
    {
        public ValueExpression(object value)
            => Value = value;

        public object Value { get; }
    }
}
