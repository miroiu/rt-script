using RTLang.Lexer;

namespace RTLang.Parser
{
    [Parslet(TokenType.Identifier)]
    public class IdentifierParslet : IParslet
    {
        public Expression Accept(Parser parser)
        {
            var identToken = parser.Take();

            if (parser.Current.Type == TokenType.OpenParen)
            {
                parser.Take();
                var args = parser.ParseArguments(TokenType.CloseParen);
                parser.Match(TokenType.CloseParen);

                return new InvocationExpression(identToken.Text, args)
                {
                    Token = identToken
                };
            }
            else if (parser.Current.Type == TokenType.OpenBrace)
            {
                parser.Take();
                var arg = parser.ParsePrimaryExpression();
                parser.Match(TokenType.CloseBrace);

                return new IndexerExpression(identToken.Text, arg)
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
