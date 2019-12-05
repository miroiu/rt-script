using System;
using System.Collections.Generic;

namespace RTScript
{
    public class CommandInfo : IEquatable<CommandInfo>
    {
        public CommandInfo(string name, string description = default, List<CommandOptionAttribute> options = default)
        {
            Options = options ?? new List<CommandOptionAttribute>();
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
        public List<CommandOptionAttribute> Options { get; }

        public bool Equals(CommandInfo other)
            => other.Name == Name;

        public override bool Equals(object obj)
            => obj is CommandInfo c && Equals(c);

        public override int GetHashCode()
            => Name.GetHashCode();
    }
}
