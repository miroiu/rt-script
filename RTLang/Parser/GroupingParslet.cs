using RTScript.Expressions;
using RTScript.Lexer;

namespace RTScript.Parser
{
    [Parslet(TokenType.OpenParen)]
    public class GroupingParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
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
