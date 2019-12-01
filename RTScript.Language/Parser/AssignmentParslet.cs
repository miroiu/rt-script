﻿using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    [Parslet(TokenType.Identifier, true)]
    public class AssignmentParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
        {
            var id = parser.Take();
            var equalsToken = parser.Match(TokenType.Equals);

            var expr = parser.ParseExpression();
            // TODO: Parse identifier
            var identifier = new IdentifierExpression(id.Text);

            return new AssignmentExpression(identifier, expr)
            {
                Token = equalsToken
            };
        }
    }
}
