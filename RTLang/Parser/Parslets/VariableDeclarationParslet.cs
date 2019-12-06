using RTLang.Lexer;

namespace RTLang.Parser
{
    [Parslet(TokenType.Var, true)]
    [Parslet(TokenType.Const, true)]
    public class VariableDeclarationParslet : IParslet
    {
        public Expression Accept(IRTLangParser parser)
        {
            var varToken = parser.Take();
            var identifierToken = parser.Match(TokenType.Identifier);

            parser.Match(TokenType.Equals);
            var initializer = parser.ParseExpression();
            bool isReadOnly = varToken.Type == TokenType.Const ? true : false;

            return new VariableDeclarationExpression(isReadOnly, identifierToken.Text, initializer)
            {
                Token = identifierToken
            };
        }
    }
}
