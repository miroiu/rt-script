namespace RTLang.Parser
{
    public class GroupingExpression : Expression
    {
        public GroupingExpression(Expression inner)
        {
            Inner = inner;
        }

        public Expression Inner { get; }

        public override void Accept(ILangVisitor<Expression> visitor)
        {
            visitor.Visit(Inner);
        }
    }
}
