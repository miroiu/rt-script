﻿using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    [Parslet(TokenType.OpenBrace)]
    public class ArrayParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
        {
            var openBrace = parser.Take();

            var args = parser.ParseArguments(TokenType.CloseBrace);

            if (args.Items.Count == 0)
            {
                throw new ParserException(openBrace, "Empty arrays are not allowed.");
            }

            parser.Match(TokenType.CloseBrace);

            return new ArrayExpression(args)
            {
                Token = openBrace
            };
        }
    }
}