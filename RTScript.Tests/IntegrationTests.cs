using RTScript.Language.Interpreter;
using RTScript.Language.Lexer;
using RTScript.Language.Parser;
using NUnit.Framework;
using System;
using RTScript.Tests.Mocks;
using RTScript.Language.Interop;
using System.Collections.Generic;

namespace RTScript.Tests
{
    public static class App
    {
        public static string[] Run(string code, RTScriptInterpreter interp = default, MockOutputStream outputStream = default)
        {
            var mockOutput = outputStream ?? new MockOutputStream();
            var source = new SourceText(code);
            var lexer = new RTScriptLexer(source);
            var parser = new RTScriptParser(lexer);

            var interpreter = interp ?? new RTScriptInterpreter(mockOutput);
            interpreter.Run(parser);
            return mockOutput.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    [TestFixture]
    public class IntegrationTests
    {
        public RTScriptInterpreter Interpreter { get; set; }
        public MockOutputStream Output = new MockOutputStream();
        public TestClass Test = new TestClass();

        [SetUp]
        public void Setup()
        {
            Output.Clear();
            Interpreter = new RTScriptInterpreter(Output);
            Interpreter.Context.Declare("test", Test);
        }

        [Test]
        [TestCase("print test.InstanceProp;", new string[] { "null" })]
        [TestCase("test.InstanceProp = 'Instance'; print test.InstanceProp;", new string[] { "Instance" })]
        [TestCase("print test.StaticProp;", new string[] { "Static" })]
        [TestCase("test.StaticProp = null; print test.StaticProp;", new string[] { "null" })]
        [TestCase("print test.StaticMethod();", new string[] { "true" })]
        [TestCase("print test.InstanceMethod(1.0);", new string[] { "-1" })]
        [TestCase("test.Ints['one'] = 1; print test.Ints['one'];", new string[] { "1" })]
        public void Interop(string input, string[] expected)
        {
            var result = App.Run(input, Interpreter, Output);
            Assert.AreEqual(expected, result);
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
