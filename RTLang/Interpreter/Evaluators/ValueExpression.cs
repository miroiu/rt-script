using RTLang.Expressions;
using System;

namespace RTLang.Interpreter.Evaluators
{
    // Used only by the evaluators.
    // Leaf node in the expression tree
    public class ValueExpression : Expression
    {
        public ValueExpression(object value, Type type)
        {
            Value = value;
            Type = type;
        }

        public object Value { get; }
        public Type Type { get; }
    }
}
