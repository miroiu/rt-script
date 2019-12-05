using NUnit.Framework;
using RTLang.Parser;
using RTLang.Lexer;
using RTLang.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

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
        // Semicolon
        [TestCase("var a = 1", typeof(ParserException))]
        [TestCase("print a", typeof(ParserException))]
        // Reserved keywords
        [TestCase("var print;", typeof(ParserException))]
        [TestCase("var var;", typeof(ParserException))]
        [TestCase("var null;", typeof(ParserException))]
        [TestCase("var true;", typeof(ParserException))]
        [TestCase("var false;", typeof(ParserException))]
        [TestCase("var const;", typeof(ParserException))]
        // Variable declaration
        [TestCase("var a;", typeof(ParserException))]
        [TestCase("var =;", typeof(ParserException))]
        [TestCase("var a=;", typeof(ParserException))]
        [TestCase("const a;", typeof(ParserException))]
        [TestCase("const =;", typeof(ParserException))]
        [TestCase("const a=;", typeof(ParserException))]
        [TestCase("var;", typeof(ParserException))]
        [TestCase("const;", typeof(ParserException))]
        [TestCase("const 1;", typeof(ParserException))]
        [TestCase("const +;", typeof(ParserException))]
        // Variable initialization
        [TestCase("a =;", typeof(ParserException))]
        [TestCase("a =", typeof(ParserException))]
        [TestCase("a =", typeof(ParserException))]
        // Array creation
        [TestCase("var x = [;", typeof(ParserException))]
        [TestCase("var x = [", typeof(ParserException))]
        [TestCase("var x = [[];", typeof(ParserException))]
        [TestCase("var x = [[]", typeof(ParserException))]
        [TestCase("var x = []];", typeof(ParserException))]
        [TestCase("var x = []]", typeof(ParserException))]
        [TestCase("var x = [,1];", typeof(ParserException))]
        [TestCase("var x = [,1]", typeof(ParserException))]
        public void Exceptions(string input, Type exType)
        {
            Assert.Throws(exType, () => ParserApp.Run(input));
        }

        [Test]
        [TestCase("const a = 1;", typeof(VariableDeclarationExpression))]
        [TestCase("var a = 1;", typeof(VariableDeclarationExpression))]
        [TestCase("var a = b;", typeof(VariableDeclarationExpression))]
        [TestCase("a = (1 + 2);", typeof(BinaryExpression))]
        [TestCase("a = (1 + 2);", typeof(BinaryExpression))]
        [TestCase("print 1;", typeof(UnaryExpression))]
        public void RootExpression(string input, Type expressionType)
        {
            Assert.AreEqual(new[] { expressionType }, ParserApp.Run(input).Select(t => t.GetType()).ToArray());
        }
    }
}