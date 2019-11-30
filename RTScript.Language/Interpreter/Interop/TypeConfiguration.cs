using System;
using System.Collections.Generic;

namespace RTScript.Language.Interpreter.Interop
{
    // TODO: Make this configurable to wrap only certain properties (marked with attributes or manually added)
    public class TypeConfiguration : IEquatable<Type>, IEquatable<TypeConfiguration>
    {
        public TypeConfiguration()
        {
            Properties = new List<string>();
            IsConfigured = true;
        }

        public Type Type { get; set; }

        public bool IsConfigured { get; set; }

        public List<string> Properties { get; }

        #region Equality

        public bool Equals(Type other)
            => other == Type;

        public bool Equals(TypeConfiguration other)
            => other.Type == Type;

        public override int GetHashCode()
            => Type.GetHashCode();

        public override bool Equals(object obj)
            => obj is TypeConfiguration tc ? Equals(tc) : obj is Type t ? Equals(t) : false;

        #endregion
    }
}
