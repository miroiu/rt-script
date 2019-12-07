using System.Collections.Generic;

namespace RTLang.Tests.Mocks
{
    public class CMock
    {
        public static void StaticMethod1() { }
        public static void StaticMethod2() { }
        public static int StaticProperty { get; }

        public bool HasDepth(int value) => Depth.Contains(value);

        public List<int> Depth { get; } = new List<int>();
    }
}
