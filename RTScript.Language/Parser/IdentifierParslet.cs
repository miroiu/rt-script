using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    [Parslet(TokenType.Identifier)]
    public class IdentifierParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
        {
            var identToken = parser.Take();
            return new Identifier(identToken.Text)
            {
                Token = identToken
            };
        }
    }
}
