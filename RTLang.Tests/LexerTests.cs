using NUnit.Framework;
using RTLang.Lexer;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.Tests
{
    public static class LexerApp
    {
        public static Token[] Run(string input)
        {
            var sourceText = new SourceText(input);
            var lexer = new Lexer.Lexer(sourceText);

            var tokens = new List<Token>();
            Token t = lexer.Lex();
            tokens.Add(t);

            while (t.Type != TokenType.EndOfCode)
            {
                t = lexer.Lex();
                tokens.Add(t);
            }

            return tokens.ToArray();
        }
    }

    public class LexerTests
    {
        [Test]
        [TestCase("var a = 1;", new[] { TokenType.Var, TokenType.Identifier, TokenType.Equals, TokenType.Number, TokenType.Semicolon, TokenType.EndOfCode })]
        public void TokenTypes(string input, TokenType[] tokenTypes)
        {
            Assert.AreEqual(tokenTypes, LexerApp.Run(input).Select(t => t.Type).ToArray());
        }
    }
}