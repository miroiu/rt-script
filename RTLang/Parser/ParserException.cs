using RTLang.Lexer;

namespace RTLang.Parser
{
    public class ParserException : RTLangException
    {
        public ParserException(Token token, string message) : base(message)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}
