using RTLang.Lexer;

namespace RTLang.Parser
{
    [Parslet(TokenType.Semicolon, true)]
    [Parslet(TokenType.EndOfCode, true)]
    public class EmptyParslet : IParslet
    {
        public Expression Accept(IRTLangParser parser)
        {
            return new EmptyExpression()
            {
                Token = parser.Current
            };
        }
    }
}
