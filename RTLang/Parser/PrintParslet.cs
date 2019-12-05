using RTLang.Lexer;

namespace RTLang.Parser
{
    [Parslet(TokenType.Print, true)]
    public class PrintParslet : IParslet
    {
        public Expression Accept(Parser parser)
        {
            var opToken = parser.Take();
            var expr = parser.ParseExpression();

            return new UnaryExpression(UnaryOperatorType.Print, expr)
            {
                Token = opToken
            };
        }
    }
}
