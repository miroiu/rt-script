namespace RTLang.Lexer
{
    public class LexerException : RTLangException
    {
        public LexerException(int line, int column, string message) : base(message)
        {
            Line = line;
            Column = column;
        }

        public int Line { get; }
        public int Column { get; }
    }
}
