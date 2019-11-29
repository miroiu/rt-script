using RTScript.Language.Interpreter;
using RTScript.Language.Lexer;
using RTScript.Language.Parser;
using RTScript.Tests.Mocks;
using NUnit.Framework;

namespace RTScript.Tests
{
    public class LexerTests
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
            var lexer = new RTScriptLexer(source);
            var parser = new RTScriptParser(lexer);
            var interpreter = new RTScriptInterpreter(_out);
            interpreter.Run(parser);
            var output = _out.Output.ToString();

            Assert.AreEqual(output, "2\r\n");
        }
    }
}