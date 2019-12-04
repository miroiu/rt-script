using RTLang.Tests.Mocks;
using NUnit.Framework;
using RTLang.Interpreter;
using System;

namespace RTLang.Tests
{
    public static class InterpreterApp
    {
        public static string[] Run(string input, IExecutionContext context, MockOutputStream output)
        {
            var interpreter = new Interpreter.Interpreter(context);
            var source = new Lexer.SourceText(input);
            var lexer = new Lexer.Lexer(source);
            var parser = new Parser.Parser(lexer);

            interpreter.Run(parser);

            return output.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class InterpreterTests
    {
        public MockOutputStream Output { get; private set; }
        public IExecutionContext Context { get; private set; }

        [SetUp]
        public void Setup()
        {
            Output = new MockOutputStream();
            Context = RTScript.NewContext(Output);
        }

        [Test]
        [TestCase("print a;", typeof(ExecutionException))]
        public void Exceptions(string input, Type exType)
        {
            Assert.Throws(exType, () => InterpreterApp.Run(input, Context, Output));
        }

        [Test]
        [TestCase("var a = 2; print a;", new string[] { "2" })]
        [TestCase(";;;;;;;;;", new string[0])]
        [TestCase("var _ = 2; print _;", new string[] { "2" })]
        [TestCase("print 5.0 + 3.0;", new string[] { "8" })]
        [TestCase("print 5.0 - 3.0;", new string[] { "2" })]
        [TestCase("print 5.0 / 2.0;", new string[] { "2.5" })]
        [TestCase("print 3.0 / 3.0;", new string[] { "1" })]
        [TestCase("print 5.0 * 2.0;", new string[] { "10" })]
        [TestCase("print !true;", new string[] { "false" })]
        [TestCase("var a = 5.0; print 10.0 / a;", new string[] { "2" })]
        [TestCase("// some comment; // print 5;", new string[0])]
        [TestCase("var a = [1, 2, 3]; print a;", new string[] { "[1, 2, 3]" })]
        [TestCase("var a = 2; print a; // print a + 1;", new string[] { "2" })]
        public void Everything(string input, string[] expected)
        {
            var result = InterpreterApp.Run(input, Context, Output);
            Assert.AreEqual(expected, result);
        }
    }
}