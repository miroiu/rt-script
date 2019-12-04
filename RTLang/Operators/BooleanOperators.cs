namespace RTScript
{
    public static class BooleanOperators
    {
        [Operator(UnaryOperatorType.LogicalNegation)]
        public static bool Negate(bool value) => !value;
    }
}
