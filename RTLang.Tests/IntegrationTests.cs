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

        [SetUp]
        public void Setup()
        {
            Output = new MockOutputStream();
            Context = RTScript.NewContext(Output);
            Context.Declare("test", new TestClass());
            Context.Declare("testc", typeof(TestClass));
        }

        [Test]
        [TestCase("print test.InstanceProp;", new string[] { "null" })]
        [TestCase("test.InstanceProp = 'Instance'; print test.InstanceProp;", new string[] { "\"Instance\"" })]
        [TestCase("print testc.StaticProp;", new string[] { "\"Static\"" })]
        [TestCase("testc.StaticProp = null; print testc.StaticProp;", new string[] { "null" })]
        [TestCase("print testc.StaticMethod();", new string[] { "true" })]
        [TestCase("print test.InstanceMethod(1.0);", new string[] { "-1" })]
        [TestCase("test.Ints['one'] = 1; print test.Ints['one'];", new string[] { "1" })]
        public void Interop(string input, string[] expected)
        {
            var result = ScriptApp.Run(input, Context, Output);
            Assert.AreEqual(expected, result);
        }
    }
}
