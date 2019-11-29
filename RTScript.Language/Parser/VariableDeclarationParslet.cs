using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    [Parslet(TokenType.Var, true)]
    [Parslet(TokenType.Const, true)]
    public class VariableDeclarationParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
        {
            var varToken = parser.Take();
            var identifierToken = parser.Match(TokenType.Identifier);

            parser.Match(TokenType.Equals);
            var initializer = parser.ParseExpression();
            var id = new Identifier(identifierToken.Text);

            return new VariableDeclaration(id, initializer)
            {
                Token = varToken
            };
        }
    }
}
