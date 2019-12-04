using RTScript.Expressions;
using RTScript.Lexer;

namespace RTScript.Parser
{
    [Parslet(TokenType.Print, true)]
    public class PrintParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
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
