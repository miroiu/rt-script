using NUnit.Framework;
using RTLang.Lexer;
using System;
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
        // Keywords
        [TestCase("var", new[] { TokenType.Var, TokenType.EndOfCode })]
        [TestCase("const", new[] { TokenType.Const, TokenType.EndOfCode })]
        [TestCase("print", new[] { TokenType.Print, TokenType.EndOfCode })]
        [TestCase("true", new[] { TokenType.True, TokenType.EndOfCode })]
        [TestCase("false", new[] { TokenType.False, TokenType.EndOfCode })]
        [TestCase("null", new[] { TokenType.Null, TokenType.EndOfCode })]
        // String
        [TestCase("'text'", new[] { TokenType.String, TokenType.EndOfCode })]
        [TestCase("'text \"quoted\"'", new[] { TokenType.String, TokenType.EndOfCode })]
        [TestCase("\"text 'quoted'\"", new[] { TokenType.String, TokenType.EndOfCode })]
        [TestCase("\"text\" 'quoted'", new[] { TokenType.String, TokenType.String, TokenType.EndOfCode })]
        // Number
        [TestCase("1.25", new[] { TokenType.Number, TokenType.EndOfCode })]
        [TestCase("1", new[] { TokenType.Number, TokenType.EndOfCode })]
        [TestCase(".1", new[] { TokenType.Dot, TokenType.Number, TokenType.EndOfCode })]
        // Identifier
        [TestCase("v", new[] { TokenType.Identifier, TokenType.EndOfCode })]
        [TestCase("_v", new[] { TokenType.Identifier, TokenType.EndOfCode })]
        [TestCase("v_", new[] { TokenType.Identifier, TokenType.EndOfCode })]
        [TestCase("_v_", new[] { TokenType.Identifier, TokenType.EndOfCode })]
        [TestCase("_", new[] { TokenType.Identifier, TokenType.EndOfCode })]
        [TestCase("v1", new[] { TokenType.Identifier, TokenType.EndOfCode })]
        [TestCase("v_1", new[] { TokenType.Identifier, TokenType.EndOfCode })]
        [TestCase("v1_", new[] { TokenType.Identifier, TokenType.EndOfCode })]
        [TestCase("1a", new[] { TokenType.Number, TokenType.Identifier, TokenType.EndOfCode })]
        // Operators
        [TestCase("+", new[] { TokenType.Plus, TokenType.EndOfCode })]
        [TestCase("-", new[] { TokenType.Minus, TokenType.EndOfCode })]
        [TestCase("*", new[] { TokenType.Asterisk, TokenType.EndOfCode })]
        [TestCase("/", new[] { TokenType.Slash, TokenType.EndOfCode })]
        [TestCase(">", new[] { TokenType.GreaterThan, TokenType.EndOfCode })]
        [TestCase("<", new[] { TokenType.LessThan, TokenType.EndOfCode })]
        [TestCase("=", new[] { TokenType.Equals, TokenType.EndOfCode })]
        [TestCase("==", new[] { TokenType.EqualsEquals, TokenType.EndOfCode })]
        [TestCase("!=", new[] { TokenType.ExclamationEquals, TokenType.EndOfCode })]
        [TestCase("!", new[] { TokenType.Exclamation, TokenType.EndOfCode })]
        [TestCase(".", new[] { TokenType.Dot, TokenType.EndOfCode })]
        // Separators
        [TestCase("(", new[] { TokenType.OpenParen, TokenType.EndOfCode })]
        [TestCase(")", new[] { TokenType.CloseParen, TokenType.EndOfCode })]
        [TestCase("[", new[] { TokenType.OpenBrace, TokenType.EndOfCode })]
        [TestCase("]", new[] { TokenType.CloseBrace, TokenType.EndOfCode })]
        [TestCase(",", new[] { TokenType.Comma, TokenType.EndOfCode })]
        [TestCase(";", new[] { TokenType.Semicolon, TokenType.EndOfCode })]
        // Whitespace
        [TestCase("\r\n", new[] { TokenType.EndOfCode })]
        [TestCase("\t", new[] { TokenType.EndOfCode })]
        [TestCase("", new[] { TokenType.EndOfCode })]
        [TestCase(" ", new[] { TokenType.EndOfCode })]
        [TestCase(" 1", new[] { TokenType.Number, TokenType.EndOfCode })]
        [TestCase(" 1 ", new[] { TokenType.Number, TokenType.EndOfCode })]

        [TestCase("var a = 1;", new[] { TokenType.Var, TokenType.Identifier, TokenType.Equals, TokenType.Number, TokenType.Semicolon, TokenType.EndOfCode })]
        public void TokenTypes(string input, TokenType[] tokenTypes)
        {
            Assert.AreEqual(tokenTypes, LexerApp.Run(input).Select(t => t.Type).ToArray());
        }

        [Test]
        // Unknown characters
        [TestCase("%", typeof(LexerException))]
        // Numbers
        [TestCase("12.", typeof(LexerException))]
        [TestCase("1.2.", typeof(LexerException))]
        [TestCase("1..2", typeof(LexerException))]
        [TestCase("1..2", typeof(LexerException))]
        // Strings
        [TestCase("'asd", typeof(LexerException))]
        [TestCase("asd'", typeof(LexerException))]
        [TestCase("asd\"", typeof(LexerException))]
        [TestCase("\"asd", typeof(LexerException))]
        [TestCase("\"asd'", typeof(LexerException))]
        [TestCase("'asd\"", typeof(LexerException))]
        public void Exceptions(string input, Type exceptionType)
        {
            Assert.Throws(exceptionType, () => LexerApp.Run(input));
        }
    }
}