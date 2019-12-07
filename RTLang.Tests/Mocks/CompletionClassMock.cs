namespace RTLang.Tests.Mocks
{
    public class CompletionClassMock
    {
        public static void StaticMethod1() { }
        public static void StaticMethod2() { }
        public static int StaticProperty { get; }

        public bool IsDeep(bool x) => x;
        public bool IsDeepProp => true;
    }
}
