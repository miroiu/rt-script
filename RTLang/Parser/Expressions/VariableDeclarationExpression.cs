namespace RTLang.Parser
{
    public class VariableDeclarationExpression : Expression
    {
        public VariableDeclarationExpression(bool isReadOnly, string identifier, Expression initializer)
        {
            IsReadOnly = isReadOnly;
            Identifier = identifier;
            Initializer = initializer;
        }

        public bool IsReadOnly { get; }
        public string Identifier { get; }
        public Expression Initializer { get; }

        public override void Accept(ILangVisitor<Expression> visitor)
        {
            visitor.Visit(Initializer);
        }
    }
}
