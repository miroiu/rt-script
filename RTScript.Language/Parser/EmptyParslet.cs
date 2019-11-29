using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
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
