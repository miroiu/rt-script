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
            _context.Declare<CMock>();
            _context.Declare("mock", new CMock());
            _context.Declare("mockInt", 1);
            _context.Declare("mockIntConst", 1, true);
        }

        [Test]
        [TestCase("", 0, new string[] { "CMock" })]
        [TestCase(";", 0, new string[] { "CMock" })]
        [TestCase(";", 1, new string[] { "CMock" })]
        // Accessors
        [TestCase("CMock.StaticMeth", 16, new string[] { "StaticMethod1", "StaticMethod2" })]
        [TestCase("CMock.StaticProperty + CMock.StaticMeth", 39, new string[] { "StaticMethod1", "StaticMethod2" })]
        [TestCase("CMock.StaticProperty + mock.HasDepth(1).ToStr", 45, new string[] { "ToString" })]
        [TestCase("CMock.StaticProperty + mock.Depth.ToStr", 39, new string[] { "ToString" })]
        [TestCase("CMock.StaticProperty + mock.Depth[int.Parse('0')].ToStr", 55, new string[] { "ToString" })]
        // Identifier
        [TestCase("v", 1, new string[] { "var" })]
        [TestCase("const", 3, new string[] { "const" })]
        [TestCase("const", 5, new string[0])]
        // Variable declaration
        [TestCase("var ", 4, new string[0])]
        [TestCase("var x", 4, new string[0])]
        [TestCase("var x = ", 8, new string[] { "mockInt" })]
        [TestCase("var x = mo", 10, new string[] { "mock", "mockInt" })]
        // Assignment
        [TestCase("mock = mockI", 12, new string[] { "mockInt" })]
        [TestCase("var x = 1 + (mock = mockI", 22, new string[] { "mockInt" })]
        [TestCase("var x = -mo", 11, new string[] { "mockInt" })]
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
        [TestCase("var CMock = 1;", new string[] { "CMock" })]
        [TestCase("var x;", new string[] { ";" })]
        [TestCase("var x = +;", new string[] { "+" })]
        [TestCase("var x = x;", new string[] { "x" })]
        [TestCase("const", new string[] { "" })]
        [TestCase("const;", new string[] { ";" })]
        [TestCase("const var;", new string[] { "var" })]
        // Assignment
        [TestCase("CMock = 5;", new string[] { "CMock" })]
        [TestCase("mockIntConst = 3;", new string[] { "mockIntConst" })]
        public void Diagnostics(string input, string[] expected)
        {
            var result = _service.GetDiagnostics(input);
            var errors = result.Select(e => input.Substring(e.Position, e.Length)).ToArray();
            Assert.AreEqual(expected, errors);
        }
    }
}
