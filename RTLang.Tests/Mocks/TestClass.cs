﻿using System.Collections.Generic;

namespace RTLang.Tests.Mocks
{
    public class TestClass
    {
        public static string StaticProp { get; set; } = "Static";
        public string InstanceProp { get; set; }

        public Dictionary<string, int> Ints { get; } = new Dictionary<string, int>()
        {
            ["one"] = 1
        };

        public static bool StaticMethod()
            => true;

        public double InstanceMethod(double d)
            => -d;

        public static int Overload(int x) => x;
        public static int Overload(int x, int y) => x + y;
        public static string Overload(int x, string y) => x + y;
    }
}
