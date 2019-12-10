using System;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    public struct Completion : IEquatable<Completion>
    {
        public string Text { get; set; }
        public SymbolType Type { get; set; }

        public override bool Equals(object obj)
            => obj is Completion c && c.Equals(this);

        public bool Equals(Completion other)
            => Type == other.Type && Text == other.Text;

        public override int GetHashCode()
        {
            var hashCode = 467094139;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Completion left, Completion right)
            => left.Equals(right);

        public static bool operator !=(Completion left, Completion right)
            => !(left == right);
    }
}