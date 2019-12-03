namespace RTScript.Language.Expressions
{
    public class InvocationExpression : Expression
    {
        public InvocationExpression(string name, ArgumentsExpression arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public string Name { get; }
        public ArgumentsExpression Arguments { get; }
    }
}
