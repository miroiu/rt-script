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
            var source = new Lexer.SourceText(input);
            var lexer = new Lexer.Lexer(source);
            var parser = new Parser.Parser(lexer);
            var interpreter = new Interpreter.Interpreter(parser);

            interpreter.Run(context);

            return output.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class InterpreterTests
    {
        public MockOutputStream Output { get; private set; }
        public IExecutionContext Context { get; private set; }
        public Action Action { get; } = () => { };

        [SetUp]
        public void Setup()
        {
            Output = new MockOutputStream();
            Context = RTScript.NewContext(Output);

            Context.Declare("action", Action);
        }

        [Test]
        // Context
        [TestCase("print a;")]
        [TestCase("print true + 1;")]
        [TestCase("print -true;")]
        // Variables
        [TestCase("var a = null;")]
        [TestCase("const a = 2; a = 1;")]
        [TestCase("const a = 2; a = 1;")]
        [TestCase("var a = 2; a = 'asd';")]
        [TestCase("var a = 1; var a = 2;")]
        // Array
        [TestCase("const a = [null, 2];")]
        [TestCase("const a = [2, '3'];")]
        // Accessor
        [TestCase("var a = 'str'; a = null; print a.ToString();")]
        [TestCase("var a = 'str'; a = null; print a.Length;")]
        [TestCase("var a = 'str'; a = null; print a.Length[0];")]
        [TestCase("var a = 'str'; print a.ToString(1);")]
        [TestCase("var a = 'str'; print a.NonExisting;")]
        // Invocation
        [TestCase("action(1);")]
        [TestCase("action = null; action();")]
        // Indexer
        [TestCase("var a = 'str'; print a['asd'];")]
        [TestCase("var a = 'str'; print a[null];")]
        [TestCase("var a = 'str'; a = null; print a[null];")]
        // Operators
        [TestCase("print -null;")]
        [TestCase("print 1 + null;")]
        [TestCase("var a = 1; print 1 + a = 2;")]
        public void Exceptions(string input)
        {
            Assert.Throws(typeof(ExecutionException), () => InterpreterApp.Run(input, Context, Output));
        }

        [Test]
        // Variables
        [TestCase("var a = 2; print a;", new string[] { "2" })]
        [TestCase("var a = 2; var b = a + 1; print b; print a / 2;", new string[] { "3", "1" })]
        [TestCase("var a = [1, 2, 3]; print a;", new string[] { "[1, 2, 3]" })]
        [TestCase("action();", new string[0])]
        // Syntax
        [TestCase(";;;;;;;;;", new string[0])]
        [TestCase("// some comment; // print 5;", new string[0])]
        [TestCase("print 1; // print 1 + 1;", new string[] { "1" })]
        // Operators
        [TestCase("print '1' + (5.0 + 3 - (1 * -2)) / 2.5;", new string[] { "14" })]
        [TestCase("print !true;", new string[] { "false" })]
        [TestCase("print 1 == 1;", new string[] { "true" })]
        [TestCase("print 1 != 1;", new string[] { "false" })]
        [TestCase("print 1 > 1;", new string[] { "false" })]
        [TestCase("print 1 < 1;", new string[] { "false" })]
        [TestCase("var a = 1; print a + (a = 2);", new string[] { "3" })]
        public void Everything(string input, string[] expected)
        {
            var result = InterpreterApp.Run(input, Context, Output);
            Assert.AreEqual(expected, result);
        }
    }
}