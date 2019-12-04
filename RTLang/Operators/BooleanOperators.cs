namespace RTLang.Operators
{
    public static class BooleanOperators
    {
        [Operator(UnaryOperatorType.LogicalNegation)]
        public static bool Negate(bool value) => !value;
    }
}
