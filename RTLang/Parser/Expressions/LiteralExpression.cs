namespace RTLang.Parser
{
    public class LiteralExpression : Expression
    {
        public LiteralExpression(LiteralType type, string value)
        {
            Type = type;
            Value = value;
        }

        public LiteralType Type { get; }
        public string Value { get; }
    }
}
