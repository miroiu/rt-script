using System;
using System.Collections.Generic;

namespace RTLang.Interop
{
    public class PropertyDescriptor
    {
        private readonly int _hashCode;

        public PropertyDescriptor(string name, Type propertyType, Type parameterType = default, bool canRead = true, bool canWrite = true, bool isStatic = false, bool isIndexer = false)
        {
            Name = name;
            CanRead = canRead;
            CanWrite = canWrite;
            IsStatic = isStatic;
            ReturnType = propertyType;
            ParameterType = parameterType ?? propertyType;
            IsIndexer = isIndexer;

            _hashCode = -1319975522;
            _hashCode = _hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            _hashCode = _hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(ReturnType);
            _hashCode = _hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(ParameterType);
        }

        public string Name { get; }
        public bool CanRead { get; }
        public bool CanWrite { get; }
        public bool IsStatic { get; }
        public bool IsIndexer { get; }

        public Type ReturnType { get; }
        public Type ParameterType { get; }

        public override bool Equals(object obj)
            => obj is PropertyDescriptor p && p.GetHashCode() == GetHashCode();

        public override int GetHashCode()
            => _hashCode;
    }
}
