using NUnit.Framework;
using RTLang.CodeAnalysis;
using RTLang.Tests.Mocks;
using System;
using System.Linq;

namespace RTLang.Tests
{
    [TestFixture]
    public class CodeAnalysisTests
    {
        public AnalysisContext Context { get; private set; }
        public RTLangService LangService { get; private set; }

        private readonly Action _invocation = () => { };

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Context = RTLangService.NewContext(RTScript.NewContext(new MockOutputStream()));
            LangService = RTLangService.Create(Context);

            Context.Declare<CMock>();
            Context.Declare<int>();
            Context.Declare("mock", new CMock());
            Context.Declare("mockInt", 1);
            Context.Declare("mockIntConst", 1, true);
            Context.Declare("inv", _invocation);
            Context.Declare<ColorsEnum>();
            Context.Declare("gray", ColorsEnum.Gray);
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
        [TestCase("mock.Depth.C", 55, new string[] { "Count", "Capacity" })]
        // Identifier
        [TestCase("v", 1, new string[] { "var" })]
        [TestCase("const", 3, new string[] { "const" })]
        [TestCase("const", 5, new string[0])]
        [TestCase("moc", 5, new string[] { "mock", "mockInt" })]
        // Variable declaration
        [TestCase("var ", 4, new string[0])]
        [TestCase("var x", 4, new string[0])]
        [TestCase("var x = ", 8, new string[] { "mockInt" })]
        [TestCase("var x = mo", 10, new string[] { "mock", "mockInt" })]
        [TestCase("var x = -mo", 11, new string[] { "mockInt" })]
        // Assignment
        [TestCase("mock = mockI", 12, new string[] { "mockInt" })]
        [TestCase("mock = -mockI", 13, new string[] { "mockInt" })]
        [TestCase("var x = 1 + (mock = mockI", 22, new string[] { "mockInt" })]
        // Array
        [TestCase("var x = [mo]", 16, new string[] { "mock" })]
        // Groupings
        [TestCase("var x = (", 16, new string[] { "mock" })]
        // Invocation
        [TestCase("inv(", 16, new string[] { "void inv()" })]
        [TestCase("mock.HasDepth(", 45, new string[] { "bool HasDepth(int)" })]
        // Enums
        [TestCase("gray.To", 7, new string[] { "ToString" })]
        [TestCase("ColorsEnum.Gr", 15, new string[] { "Gray", "Green" })]
        public void Completions(string input, int position, string[] expected)
        {
            if (position > input.Length)
            {
                position = input.Length;
            }

            var result = LangService.GetCompletions(input, position).Select(c => c.Text).ToArray();

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
        [TestCase("CMock =;", new string[] { ";" })]
        [TestCase("CMock =", new string[] { "" })]
        [TestCase("CMock = 5;", new string[] { "CMock" })]
        [TestCase("mockIntConst = 3;", new string[] { "mockIntConst" })]
        [TestCase("mock.Depth = 3;", new string[] { "Depth" })]
        [TestCase("mock.Depth.Length = 3;", new string[] { "Length" })]
        [TestCase("CMock.StaticProperty = true;", new string[] { "StaticProperty" })]
        // Array
        [TestCase("var x = [];", new string[] { "[" })]
        [TestCase("var x = [mo];", new string[] { "mo" })]
        [TestCase("var x = [1]; print x[mo];", new string[] { "mo" })]
        // Grouping
        [TestCase("print (1 + mo) * x;", new string[] { "mo", "x" })]
        // Invocation
        [TestCase("mock.HasDepth();", new string[] { "HasDepth" })]
        [TestCase("inv(1);", new string[] { "inv" })]
        [TestCase("vv();", new string[] { "vv" })]
        [TestCase("mock.vv();", new string[] { "vv" })]
        //[TestCase("mock.HasDepth('asd');", new string[] { "'asd'" })]
        // Indexer
        [TestCase("mock.Depth[mo].Length;", new string[] { "mo" })]
        [TestCase("mock.Depth['asd'].Length;", new string[] { "'asd'" })]
        [TestCase("mock.NotExists[mo].Length;", new string[] { "NotExists" })]
        // Enums
        [TestCase("print gray.Missing;", new string[] { "Missing" })]
        [TestCase("print ColorsEnum.NonExist;", new string[] { "NonExist" })]
        public void Diagnostics(string input, string[] expected)
        {
            var result = LangService.GetDiagnostics(input);
            var errors = result.Select(e => input.Substring(e.Position, e.Length)).ToArray();
            Assert.AreEqual(expected, errors);
        }
    }
}
