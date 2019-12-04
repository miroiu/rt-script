namespace RTLang.Expressions
{
    public class IndexerExpression : Expression
    {
        public IndexerExpression(string identifierName, Expression index)
        {
            PropertyName = identifierName;
            Index = index;
        }

        public string PropertyName { get; }
        public Expression Index { get; }
    }
}
