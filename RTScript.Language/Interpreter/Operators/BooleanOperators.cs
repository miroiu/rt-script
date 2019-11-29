using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Operators
{
    public static class BooleanOperators
    {
        [UnaryOperator(UnaryOperatorType.LogicalNegation)]
        public static bool Negate(bool value) => !value;
    }
}
