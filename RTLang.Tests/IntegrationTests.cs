using NUnit.Framework;
using System;
using RTLang.Tests.Mocks;

namespace RTLang.Tests
{
    public static class ScriptApp
    {
        public static string[] Run(string code, IExecutionContext context, MockOutputStream output)
        {
            RTScript.Execute(code, context);
            return output.Output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    [TestFixture]
    public class IntegrationTests
    {
        public MockOutputStream Output { get; private set; }
        public IExecutionContext Context { get; private set; }

        private readonly TestClass _test = new TestClass();

        [SetUp]
        public void Setup()
        {
            Output = new MockOutputStream();
            Context = RTScript.NewContext(Output);
            Context.Declare("t", _test);
            Context.Declare(typeof(TestClass));
        }

        [Test]
        [TestCase("print t.InstanceProp;", new string[] { "null" })]
        [TestCase("t.InstanceProp = 'Instance'; print t.InstanceProp;", new string[] { "Instance" })]
        [TestCase("print TestClass.StaticProp;", new string[] { "Static" })]
        [TestCase("TestClass.StaticProp = null; print TestClass.StaticProp;", new string[] { "null" })]
        [TestCase("print TestClass.StaticMethod();", new string[] { "true" })]
        [TestCase("print t.InstanceMethod(1.0);", new string[] { "-1" })]
        [TestCase("t.Ints['one'] = 1; print t.Ints['one'];", new string[] { "1" })]
        [TestCase("print TestClass.Overload(1);", new string[] { "1" })]
        [TestCase("print TestClass.Overload(1, 2);", new string[] { "3" })]
        [TestCase("print TestClass.Overload(1, '2');", new string[] { "12" })]
        public void Interop(string input, string[] expected)
        {
            var result = ScriptApp.Run(input, Context, Output);
            Assert.AreEqual(expected, result);
        }
    }
}
