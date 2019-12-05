using RTLang.Lexer;

namespace RTLang.Parser
{
    [Parslet(TokenType.String)]
    [Parslet(TokenType.Number)]
    [Parslet(TokenType.True)]
    [Parslet(TokenType.False)]
    [Parslet(TokenType.Null)]
    public class LiteralParslet : IParslet
    {
        public Expression Accept(Parser parser)
        {
            var literal = parser.Take();

            return new LiteralExpression(GetLiteralType(literal), literal.Text)
            {
                Token = literal
            };
        }

        public static LiteralType GetLiteralType(Token token)
        {
            switch (token.Type)
            {
                case TokenType.True:
                    return LiteralType.Boolean;

                case TokenType.False:
                    return LiteralType.Boolean;

                case TokenType.Null:
                    return LiteralType.Null;

                case TokenType.Number:
                    return LiteralType.Number;

                case TokenType.String:
                    return LiteralType.String;

            }

            throw new ParserException(token, $"{token.Type} is not a literal type.");
        }
    }
}
