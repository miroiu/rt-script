namespace RTLang.Parser
{
    public class IdentifierExpression : Expression
    {
        public IdentifierExpression(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
