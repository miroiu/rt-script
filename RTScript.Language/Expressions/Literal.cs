namespace RTScript.Language.Expressions
{
    public class Literal : Expression
    {
        public Literal(LiteralType type, string value)
        {
            Type = type;
            Value = value;
        }

        public LiteralType Type { get; }
        public string Value { get; }
    }
}
