namespace RTLang.Expressions
{
    public class VariableDeclarationExpression : Expression
    {
        public VariableDeclarationExpression(bool isReadOnly, IdentifierExpression identifier, Expression initializer)
        {
            IsReadOnly = isReadOnly;
            Identifier = identifier;
            Initializer = initializer;
        }

        public bool IsReadOnly { get; }
        public IdentifierExpression Identifier { get; }
        public Expression Initializer { get; }
    }
}
