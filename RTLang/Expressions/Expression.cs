using RTLang.Lexer;

namespace RTLang.Expressions
{
    public abstract class Expression
    {
        public Token Token { get; set; }
    }
}