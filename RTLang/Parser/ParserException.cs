using RTLang.Lexer;

namespace RTLang.Parser
{
    public class ParserException : RTLangException
    {
        internal ParserException(Token token, string message) : this(token.Line, token.Column, message)
        {
        }

        public ParserException(int line, int column, string message) : base(message)
        {
            Line = line;
            Column = column;
        }

        public int Line { get; }
        public int Column { get; }
    }
}
