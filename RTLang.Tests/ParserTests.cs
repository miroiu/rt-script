using NUnit.Framework;
using RTLang.Expressions;
using RTLang.Lexer;
using RTLang.Parser;
using System;
using System.Collections.Generic;

namespace RTLang.Tests
{
    public static class ParserApp
    {
        public static Expression[] Run(string input)
        {
            var source = new SourceText(input);
            var lexer = new Lexer.Lexer(source);
            var parser = new Parser.Parser(lexer);

            var expr = new List<Expression>();
            while (parser.HasNext)
            {
                expr.Add(parser.Next());
            }

            return expr.ToArray();
        }
    }

    [TestFixture]
    public class ParserTests
    {
        [Test]
        [TestCase("var print;", typeof(ParserException))]
        [TestCase("var var;", typeof(ParserException))]
        [TestCase("var null;", typeof(ParserException))]
        [TestCase("var true;", typeof(ParserException))]
        [TestCase("var false;", typeof(ParserException))]
        [TestCase("var const;", typeof(ParserException))]
        [TestCase("var a;", typeof(ParserException))]
        public void Exceptions(string input, Type exType)
        {
            Assert.Throws(exType, () => ParserApp.Run(input));
        }
    }
}