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
        public Func<int, bool> IsEven { get; } = (i) => i % 2 == 0;

        [SetUp]
        public void Setup()
        {
            Output = new MockOutputStream();
            Context = RTScript.NewContext(Output);

            Context.Declare("action", Action);
            Context.Declare("isEven", IsEven);
            Context.Declare<int>();
        }

        [Test]
        // Context
        [TestCase("print a;")]
        [TestCase("print true + 1;")]
        [TestCase("print -true;")]
        [TestCase("a = 5;")]
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
        [TestCase("var a = 'str'; print a[a.Length];")]
        [TestCase("var a = 'str'; print a[null];")]
        [TestCase("var a = 'str'; a = null; print a[null];")]
        // Operators
        [TestCase("print -null;")]
        [TestCase("print 1 + null;")]
        [TestCase("var a = 1; print 1 + a = 2;")]
        // Statics
        [TestCase("var x = 0; print x.Parse('1');")]
        public void Exceptions(string input)
        {
            Assert.Throws(typeof(ExecutionException), () => InterpreterApp.Run(input, Context, Output));
        }

        [Test]
        // Variables
        [TestCase("var a = 2; print a;", new[] { "2" })]
        [TestCase("var a = 2; var b = a + 1; print b; print a / 2;", new[] { "3", "1" })]
        [TestCase("var a = [1, 2, 3]; print a;", new[] { "[1, 2, 3]" })]
        // Syntax
        [TestCase("", new string[0])]
        [TestCase(";;;;;;;;;", new string[0])]
        [TestCase("// some comment; // print 5;", new string[0])]
        [TestCase("print 1; // print 1 + 1;", new[] { "1" })]
        // Operators
        [TestCase("print '1' + (5.0 + 3 - (1 * -2)) / 2.5;", new[] { "14" })]
        [TestCase("print !true;", new[] { "false" })]
        [TestCase("print 1 == 1;", new[] { "true" })]
        [TestCase("print 1 != 1;", new[] { "false" })]
        [TestCase("print 1 > 1;", new[] { "false" })]
        [TestCase("print 1 < 1;", new[] { "false" })]
        [TestCase("var a = 1; print a + (a = 2);", new[] { "3" })]
        [TestCase("var a = 1; print a + a.ToString();", new[] { "11" })]
        // Indexer
        [TestCase("var a = [1, 2, 3]; print a[1];", new[] { "2" })]
        [TestCase("var a = [1, 2, 3]; print a[1] + a[0];", new[] { "3" })]
        [TestCase("var a = [1, 2, 3]; a[0] = a[2]; print a;", new[] { "[3, 2, 3]" })]
        // Statics
        [TestCase("print int.Parse('1');", new[] { "1" })]
        // Invocation
        [TestCase("action();", new string[0])]
        [TestCase("print isEven(2);", new[] { "true" })]
        // Array
        [TestCase("const a = ['asd', null, int.Parse('1')]; print a;", new[] { "[asd, null, 1]" })]
        // Accessor
        [TestCase("var a = 'str'; print a.ToString();", new[] { "str" })]
        [TestCase("var a = 'str'; print a.Length;", new[] { "3" })]
        public void Everything(string input, string[] expected)
        {
            var result = InterpreterApp.Run(input, Context, Output);
            Assert.AreEqual(expected, result);
        }
    }
}