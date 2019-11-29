using RTScript.Language.Interpreter;
using RTScript.Tests.Mocks;
using NUnit.Framework;

namespace RTScript.Tests
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
    }
}