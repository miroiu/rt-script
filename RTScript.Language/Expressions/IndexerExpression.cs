namespace RTScript.Language.Expressions
{
    public class IndexerExpression : Expression
    {
        public IndexerExpression(string name, Expression index)
        {
            Name = name;
            Index = index;
        }

        public string Name { get; }
        public Expression Index { get; }
    }
}
