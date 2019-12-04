namespace RTLang.Expressions
{
    public class IndexerExpression : Expression
    {
        public IndexerExpression(string identifierName, Expression index)
        {
            IdentifierName = identifierName;
            Index = index;
        }

        public string IdentifierName { get; }
        public Expression Index { get; }
    }
}
