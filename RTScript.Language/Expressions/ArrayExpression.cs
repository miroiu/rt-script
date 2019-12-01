namespace RTScript.Language.Expressions
{
    public class ArrayExpression : Expression
    {
        public ArrayExpression(ArgumentsExpression arguments)
        {
            Arguments = arguments;
            Length = Arguments.Items.Count;
        }

        public ArgumentsExpression Arguments { get; }
        public int Length { get; }
    }
}
