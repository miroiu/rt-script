using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Operators
{
    public static class BooleanOperators
    {
        [Operator(UnaryOperatorType.LogicalNegation)]
        public static bool Negate(bool value) => !value;
    }
}
