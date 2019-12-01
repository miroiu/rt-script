using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
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
