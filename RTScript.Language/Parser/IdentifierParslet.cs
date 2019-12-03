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

            if (parser.Current.Type == TokenType.OpenParen)
            {
                parser.Match(TokenType.OpenParen);
                var args = parser.ParseArguments(TokenType.CloseParen);
                parser.Match(TokenType.CloseParen);

                return new InvocationExpression(identToken.Text, args)
                {
                    Token = identToken
                };
            }

            return new IdentifierExpression(identToken.Text)
            {
                Token = identToken
            };
        }
    }
}
