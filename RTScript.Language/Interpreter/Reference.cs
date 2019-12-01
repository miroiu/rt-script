using System;

namespace RTScript.Language.Interpreter
{
    // Type safe reference
    public class Reference : IEquatable<Reference>, IEquatable<string>
    {
        private readonly WeakReference<object> _reference;

        public Reference(string name, bool isReadOnly, object value)
        {
            if (value == null)
            {
                throw new Exception("Cannot create a reference to a null value.");
            }

            Type = value.GetType();
            Name = name;
            IsReadOnly = isReadOnly;
            _reference = new WeakReference<object>(value);
        }

        public Type Type { get; }
        public string Name { get; }
        public bool IsReadOnly { get; }
        public object Value
        {
            get
            {
                if (_reference.TryGetTarget(out var result))
                {
                    return result;
                }

                return default;
            }
        }

        public void SetValue(object value)
        {
            if (IsReadOnly)
            {
                throw new Exception($"'{Name}' is read-only.");
            }

            if (value != null)
            {
                var newType = value.GetType();
                if (newType != Type)
                {
                    throw new Exception($"Cannot convert type '{newType.ToFriendlyName()}' to '{Type.ToFriendlyName()}'.");
                }
            }

            _reference.SetTarget(value);
        }

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
