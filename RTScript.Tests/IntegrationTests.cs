using RTScript.Language.Interpreter;
using RTScript.Language.Lexer;
using RTScript.Language.Parser;
using NUnit.Framework;
using System;

namespace RTScript.Tests
{
    public static class App
    {
        public static string[] Run(string code)
        {
            var mockOutput = new Mocks.MockOutputStream();
            var source = new SourceText(code);
            var lexer = new RTScriptLexer(source);
            var parser = new RTScriptParser(lexer);

            var interpreter = new RTScriptInterpreter(mockOutput);
            interpreter.Run(parser);
            return mockOutput.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        [TestCase("var a = 2; print a;", new string[] { "2" })]
        [TestCase(";;;;;;;;;", new string[0])]
        [TestCase("var _ = 2; print _;", new string[] { "2" })]
        [TestCase("print 5 + 3;", new string[] { "8" })]
        [TestCase("print 5 - 3;", new string[] { "2" })]
        [TestCase("print 5 / 2;", new string[] { "2.5" })]
        [TestCase("print 3 / 3;", new string[] { "1" })]
        [TestCase("print 5 * 2;", new string[] { "10" })]
        [TestCase("print !true;", new string[] { "false" })]
        [TestCase("var a = 5; print 10 / a;", new string[] { "2" })]
        [TestCase("// some comment; // print 5;", new string[0])]
        public void Everything(string input, string[] expected)
        {
            var result = App.Run(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("var print;", typeof(ParserException))]
        [TestCase("var var;", typeof(ParserException))]
        [TestCase("var null;", typeof(ParserException))]
        [TestCase("var true;", typeof(ParserException))]
        [TestCase("var false;", typeof(ParserException))]
        [TestCase("var const;", typeof(ParserException))]
        [TestCase("var a;", typeof(ParserException))]
        [TestCase("print a;", typeof(ExecutionException))]
        public void Exceptions(string input, Type exType)
        {
            Assert.Throws(exType, () => App.Run(input));
        }
    }
}
