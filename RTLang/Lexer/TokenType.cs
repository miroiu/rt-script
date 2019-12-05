namespace RTLang.Lexer
{
    public enum TokenType
    {
        EndOfCode,

        Identifier,
        Number,
        String,

        // Separators
        Comma,
        Semicolon,
        OpenParen,
        CloseParen,
        OpenBrace,
        CloseBrace,

        // Keywords
        Var,
        Const,
        Print,
        True,
        False,
        Null,

        // Operators
        Plus,
        Minus,
        Asterisk,
        Slash,
        Equals,
        Exclamation,
        Dot,
        LessThan,
        GreaterThan,
        ExclamationEquals,
        EqualsEquals,
    }
}