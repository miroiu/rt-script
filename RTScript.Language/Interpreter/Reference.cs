using System;

namespace RTScript.Language.Interpreter
{
    // TODO: Should hold weak references?
    public class Reference : IEquatable<Reference>, IEquatable<string>
    {
        public Reference(string name, bool isReadOnly, object value)
        {
            Name = name;
            IsReadOnly = isReadOnly;
            Value = value;
        }

        public string Name { get; }
        public bool IsReadOnly { get; }
        public object Value { get; private set; }

        public void SetValue(object value)
            => Value = value;

        #region Equality

        public bool Equals(Reference other)
            => other.Name == Name;

        public bool Equals(string other)
            => other == Name;

        public override bool Equals(object obj)
            => obj is Reference var ? Equals(var) : obj is string str ? Equals(str) : false;

        public override int GetHashCode()
            => Name.GetHashCode();

        #endregion
    }
}
