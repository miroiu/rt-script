using RTLang.Interpreter;
using RTLang.Tests.Mocks;
using NUnit.Framework;
using RTLang.Lexer;
using RTLang.Parser;

namespace RTLang.Tests
{
    public class InterpreterTests
    {
        private IExecutionContext _ctx;
        private MockOutputStream _out;

        [SetUp]
        public void Setup()
        {
            _out = new MockOutputStream();
            _ctx = new ExecutionContext(_out);  
        }

        [Test]
        public void Comments_Should_Be_Ignored()
        {
            var input = "var a = 2; print a; // print a + 1;";
            var source = new SourceText(input);
            var lexer = new Lexer.Lexer(source);
            var parser = new Parser.Parser(lexer);
            var interpreter = new Interpreter.Interpreter(_out);
            interpreter.Run(parser);
            var output = _out.Output.ToString();

            Assert.AreEqual(output, "2\r\n");
        }
    }
}