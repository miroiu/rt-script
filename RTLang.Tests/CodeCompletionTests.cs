using NUnit.Framework;
using RTLang.CodeAnalysis;
using RTLang.Tests.Mocks;
using System.Linq;

namespace RTLang.Tests
{
    [TestFixture]
    public class CodeCompletionTests
    {
        private RTLangService _service;
        private IExecutionContext _context;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _context = RTScript.NewContext(new MockOutputStream());
            _service = RTLangService.Create(_context);
            _context.DeclareStatic<CompletionClassMock>("mock");
            _context.Declare("mockInt", 1);
        }

        [Test]
        [TestCase("", 0, new string[] { })]
        [TestCase(";", 0, new string[] { })]
        [TestCase("v", 1, new string[] { "var" })]
        [TestCase("const", 3, new string[] { "const" })]
        [TestCase("const", 5, new string[] { })]
        [TestCase("mock.StaticMeth", 15, new string[] { "StaticMethod1", "StaticMethod2" })]
        [TestCase("mock.StaticProperty + mock.StaticMeth", 38, new string[] { "StaticMethod1", "StaticMethod2" })]
        // Variables
        [TestCase("var x = mo", 10, new string[] { "mock", "mockInt" })]
        public void Completions(string input, int position, string[] expected)
        {
            var result = _service.GetCompletions(input, position).Select(c => c.Text);
            Assert.AreEqual(expected, result.ToArray());
        }

        [TestCase("var mock = 1;", new string[] { "mock" })]
        public void Errors(string input, string[] expected)
        {
            var result = _service.GetDiagnostics(input);
            var errors = result.Select(e => input.Substring(e.Position, e.Length)).ToArray();
            Assert.AreEqual(expected, errors);
        }
    }
}
