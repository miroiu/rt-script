namespace RTLang.Parser
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

        public override void Accept(ILangVisitor<Expression> visitor)
        {
            visitor.Visit(Index);
        }
    }
}
