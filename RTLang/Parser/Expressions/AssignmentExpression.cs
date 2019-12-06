namespace RTLang.Parser
{
    public class AssignmentExpression : Expression
    {
        public AssignmentExpression(IdentifierExpression identifier, Expression initializer)
        {
            Identifier = identifier;
            Initializer = initializer;
        }

        public IdentifierExpression Identifier { get; }
        public Expression Initializer { get; }

        public override void Accept(ILangVisitor<Expression> visitor)
        {
            visitor.Visit(Identifier);
            visitor.Visit(Initializer);
        }
    }
}
