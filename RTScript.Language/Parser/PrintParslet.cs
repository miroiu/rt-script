using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    [Parslet(TokenType.Print, true)]
    public class PrintParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
        {
            var opToken = parser.Take();
            var op = opToken.ToUnaryOperatorType();
            var expr = parser.ParseExpression();

            return new UnaryExpression(op, expr)
            {
                Token = opToken
            };
        }
    }
}
