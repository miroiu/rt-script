using System;

namespace RTLang.CodeAnalysis
{
    public struct Symbol : IEquatable<Symbol>
    {
        public string Name;
        public SymbolType Type;
        public bool IsReadOnly;

        public override bool Equals(object obj)
            => obj is Symbol s && s.Equals(this);

        public bool Equals(Symbol other)
            => other.Name == Name;

        public override int GetHashCode()
            => Name.GetHashCode();

        public static bool operator ==(Symbol left, Symbol right)
            => left.Equals(right);

        public static bool operator !=(Symbol left, Symbol right)
            => !(left == right);
    }
}
