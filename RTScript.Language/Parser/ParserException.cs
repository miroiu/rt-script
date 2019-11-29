using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    public class ParserException : RTScriptException
    {
        public ParserException(Token token, string message) : base(message)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}
