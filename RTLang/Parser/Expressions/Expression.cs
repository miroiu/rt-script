using RTLang.Lexer;

namespace RTLang.Parser
{
    public abstract class Expression
    {
        public Token Token { get; set; }
    }
}