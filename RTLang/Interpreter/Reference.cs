using System;

namespace RTLang.Interpreter
{
    // Type safe weak reference
    public sealed class Reference : IEquatable<Reference>, IEquatable<string>
    {
        private readonly WeakReference<object> _reference;
        private object _hardReference;

        public Reference(string name, bool isReadOnly, object value)
        {
            if (value == default)
            {
                throw new Exception("Cannot create a reference to a null value.");
            }

            Type = value.GetType();
            Name = name;
            IsReadOnly = isReadOnly;

            if (Type.IsValueType)
            {
                _hardReference = value;
            }
            else
            {
                _reference = new WeakReference<object>(value);
            }
        }

        public Type Type { get; }
        public string Name { get; }
        public bool IsReadOnly { get; }
        public object Value
        {
            get
            {
                if (Type.IsValueType)
                {
                    return _hardReference;
                }

                _reference.TryGetTarget(out var result);
                return result;
            }

            private set
            {
                if (Type.IsValueType)
                {
                    _hardReference = value;
                }
                else
                {
                    _reference.SetTarget(value);
                }
            }
        }

        public void SetValue(object value)
        {
            if (IsReadOnly)
            {
                throw new Exception($"'{Name}' is read-only.");
            }

            if (TypeHelper.TryChangeType(ref value, Type))
            {
                Value = value;
            }
            else if (value != default)
            {
                throw new Exception($"Cannot convert type '{value.GetType().ToFriendlyName()}' to '{Type.ToFriendlyName()}'.");
            }
            else
            {
                throw new Exception($"Cannot assign null to value of type '{Type.ToFriendlyName()}'.");
            }
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
