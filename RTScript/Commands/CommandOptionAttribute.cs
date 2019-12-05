using System;

namespace RTScript
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class CommandOptionAttribute : Attribute
    {
        public CommandOptionAttribute(string name)
            => Name = name;

        public string Name { get; }
        public string Description { get; set; }
        public string Arguments { get; set; }
    }
}
