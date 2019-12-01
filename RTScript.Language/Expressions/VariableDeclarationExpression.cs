namespace RTScript.Language.Expressions
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
        // May be null (e.g var x;)
        public Expression Initializer { get; }
    }
}
