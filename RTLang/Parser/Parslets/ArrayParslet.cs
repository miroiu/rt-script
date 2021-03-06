﻿using RTLang.Lexer;

namespace RTLang.Parser
{
    [Parslet(TokenType.OpenBrace)]
    public class ArrayParslet : IParslet
    {
        public Expression Accept(IRTLangParser parser)
        {
            var openBrace = parser.Take();

            var args = parser.ParseArguments(TokenType.CloseBrace);
            parser.Match(TokenType.CloseBrace);

            return new ArrayExpression(args)
            {
                Token = openBrace
            };
        }
    }
}
