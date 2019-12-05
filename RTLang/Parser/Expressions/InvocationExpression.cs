namespace RTLang.Parser
{
    public class InvocationExpression : Expression
    {
        public InvocationExpression(string methodName, ArgumentsExpression arguments)
        {
            MethodName = methodName;
            Arguments = arguments;
        }

        public string MethodName { get; }
        public ArgumentsExpression Arguments { get; }
    }
}
