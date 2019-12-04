using System;
using System.Collections.Generic;

namespace RTScript.Interop
{
    public class MethodDescriptor
    {
        private readonly int _hashCode;

        public MethodDescriptor(string name, bool isStatic, Type returnType, params Type[] parameters)
        {
            Parameters = parameters;
            IsStatic = isStatic;
            Name = name;
            ReturnType = returnType;

            _hashCode = -583657026;
            _hashCode = _hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            _hashCode = _hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(ReturnType);
            _hashCode = _hashCode * -1521134295 + EqualityComparer<IReadOnlyList<Type>>.Default.GetHashCode(Parameters);
        }

        public string Name { get; }
        public bool IsStatic { get; }
        public Type ReturnType { get; }
        public IReadOnlyList<Type> Parameters { get; }

        public override bool Equals(object obj)
            => obj is MethodDescriptor m && m.GetHashCode() == GetHashCode();

        public override int GetHashCode()
            => _hashCode;
    }
}
