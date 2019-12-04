namespace RTLang.Expressions
{
    public class InvocationExpression : Expression
    {
        public InvocationExpression(string identifier, ArgumentsExpression arguments)
        {
            IdentifierName = identifier;
            Arguments = arguments;
        }

        public string IdentifierName { get; }
        public ArgumentsExpression Arguments { get; }
    }
}
