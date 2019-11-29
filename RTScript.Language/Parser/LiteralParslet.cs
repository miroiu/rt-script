using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    [Parslet(TokenType.String)]
    [Parslet(TokenType.Number)]
    [Parslet(TokenType.True)]
    [Parslet(TokenType.False)]
    [Parslet(TokenType.Null)]
    public class LiteralParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
        {
            var literal = parser.Take();

            return new Literal(literal.ToLiteralType(), literal.Text);
        }
    }
}
