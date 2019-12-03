namespace RTScript.Tests.Mocks
{
    public class TestClass
    {
        public static string StaticProp { get; set; } = "Static";
        public string InstanceProp { get; set; }

        public static bool StaticMethod()
            => true;

        public double InstanceMethod(double d)
            => -d;
    }
}
