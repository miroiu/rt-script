namespace RTScript.Language.Expressions
{
    public class VariableDeclaration : Expression
    {
        public VariableDeclaration(bool isReadOnly, Identifier identifier, Expression initializer)
        {
            IsReadOnly = isReadOnly;
            Identifier = identifier;
            Initializer = initializer;
        }

        public bool IsReadOnly { get; }
        public Identifier Identifier { get; }
        // May be null (e.g var x;)
        public Expression Initializer { get; }
    }
}
