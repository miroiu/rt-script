using RTLang.Lexer;

namespace RTLang.Parser
{
    [Parslet(TokenType.OpenParen)]
    public class GroupingParslet : IParslet
    {
        public Expression Accept(Parser parser)
        {
            var openParen = parser.Take();
            var expr = parser.ParseExpression();
            parser.Match(TokenType.CloseParen);

            return new GroupingExpression(expr)
            {
                Token = openParen
            };
        }
    }
}
