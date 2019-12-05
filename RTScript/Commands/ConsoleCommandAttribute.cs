using System;

namespace RTScript
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ConsoleCommandAttribute : Attribute
    {
        public ConsoleCommandAttribute(string name)
            => Name = name;

        public string Name { get; }
        public string Description { get; set; }
    }
}
