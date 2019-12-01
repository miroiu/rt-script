using System;
using System.Collections.Generic;

namespace RTScript.Language.Interpreter.Interop
{
    public class PropertyDescriptor
    {
        private readonly int _hashCode;

        public PropertyDescriptor(string name, bool canRead, bool canWrite, Type propertyType, Type parameterType = default)
        {
            Name = name;
            CanRead = canRead;
            CanWrite = canWrite;
            PropertyType = propertyType;
            ParameterType = parameterType ?? propertyType;
            IsIndexer = parameterType != default;

            _hashCode = -1319975522;
            _hashCode = _hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            _hashCode = _hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(PropertyType);
            _hashCode = _hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(ParameterType);
        }

        public string Name { get; }
        public bool CanRead { get; }
        public bool CanWrite { get; }
        public bool IsIndexer { get; }

        public Type PropertyType { get; }
        public Type ParameterType { get; }

        public override bool Equals(object obj)
            => obj is PropertyDescriptor p && p.GetHashCode() == GetHashCode();

        public override int GetHashCode()
            => _hashCode;
    }
}
