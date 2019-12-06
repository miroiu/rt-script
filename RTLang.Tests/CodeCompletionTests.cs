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
        [TestCase("", 0, new string[] { "mock" })]
        [TestCase(";", 0, new string[] { "mock" })]
        [TestCase(";", 1, new string[] { "mock" })]
        [TestCase("mock.StaticMeth", 15, new string[] { "StaticMethod1", "StaticMethod2" })]
        [TestCase("mock.StaticProperty + mock.StaticMeth", 38, new string[] { "StaticMethod1", "StaticMethod2" })]
        // Identifier
        [TestCase("v", 1, new string[] { "var" })]
        [TestCase("const", 3, new string[] { "const" })]
        [TestCase("const", 5, new string[0])]
        // Variable declaration
        [TestCase("var ", 4, new string[0])]
        [TestCase("var x", 4, new string[0])]
        [TestCase("var x = ", 8, new string[] { "mock", "mockInt" })]
        [TestCase("var x = mo", 10, new string[] { "mock", "mockInt" })]
        public void Completions(string input, int position, string[] expected)
        {
            if (position > input.Length)
            {
                position = input.Length;
            }

            var result = _service.GetCompletions(input, position).Select(c => c.Text).ToArray();

            if (result.Intersect(expected).Any())
            {
                Assert.Pass();
            }

            Assert.AreEqual(expected, result);
        }

        [TestCase("", new string[0])]
        [TestCase(";", new string[0])]
        // Variable declaration
        [TestCase("var mock = 1;", new string[] { "mock" })]
        [TestCase("var x;", new string[] { ";" })]
        [TestCase("var x = +;", new string[] { "+" })]
        [TestCase("var x = x;", new string[] { "x" })]
        [TestCase("const", new string[] { "" })]
        [TestCase("const;", new string[] { ";" })]
        [TestCase("const var;", new string[] { "var" })]
        public void Diagnostics(string input, string[] expected)
        {
            var result = _service.GetDiagnostics(input);
            var errors = result.Select(e => input.Substring(e.Position, e.Length)).ToArray();
            Assert.AreEqual(expected, errors);
        }
    }
}
