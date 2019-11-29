using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter
{
    // Used only by the evaluator.
    // Leaf node in the expression tree
    public class ValueExpression : Expression
    {
        public ValueExpression(object value)
        {
            Value = value;
        }

        public object Value { get; }
    }
}
