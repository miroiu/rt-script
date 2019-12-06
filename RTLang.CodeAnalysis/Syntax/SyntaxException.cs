using RTLang.Lexer;

namespace RTLang.CodeAnalysis.Syntax
{
    public class SyntaxException : AnalysisException
    {
        public SyntaxException(Token expected, Token current) : base($"Expected '{expected.Text}'.")
        {
            Expected = expected;
            Current = current;
        }

        public SyntaxException(Token current, string message) : base(message)
        {
            Current = current;
            Expected = Expected;
        }

        public Token Expected { get; }
        public Token Current { get; }
    }
}
