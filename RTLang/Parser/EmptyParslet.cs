using RTScript.Expressions;
using RTScript.Lexer;

namespace RTScript.Parser
{
    [Parslet(TokenType.Semicolon, true)]
    [Parslet(TokenType.EndOfCode, true)]
    public class EmptyParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
        {
            return new EmptyExpression();
        }
    }
}
