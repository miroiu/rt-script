using System;
using System.Collections.Generic;

namespace RTLang.Interop
{
    public enum DescriptorType
    {
        Indexer,
        Property,
        Enum
    }

    public class PropertyDescriptor
    {
        private readonly int _hashCode;

        public PropertyDescriptor(string name, Type propertyType, Type parameterType = default, bool canRead = true, bool canWrite = true, bool isStatic = false, DescriptorType descriptorType = DescriptorType.Property)
        {
            Name = name;
            CanRead = canRead;
            CanWrite = canWrite;
            IsStatic = isStatic;
            DescriptorType = descriptorType;
            ReturnType = propertyType;
            ParameterType = parameterType ?? propertyType;

            _hashCode = -1319975522;
            _hashCode = _hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            _hashCode = _hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(ReturnType);
            _hashCode = _hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(ParameterType);
            _hashCode = _hashCode * -1521134295 + DescriptorType.GetHashCode();
        }

        public string Name { get; }
        public bool CanRead { get; }
        public bool CanWrite { get; }
        public bool IsStatic { get; }
        public DescriptorType DescriptorType { get; }

        public Type ReturnType { get; }
        public Type ParameterType { get; }

        public override bool Equals(object obj)
            => obj is PropertyDescriptor p && p.GetHashCode() == GetHashCode();

        public override int GetHashCode()
            => _hashCode;
    }
}
