using System;
using System.Collections.Generic;

namespace RTScript.Language.Interpreter.Interop
{
    // TODO: Make this configurable to wrap only certain properties (marked with attributes or manually added)
    public class TypeConfiguration
    {
        public TypeConfiguration(Type type)
        {
            Properties = new List<PropertyDescriptor>();
            Methods = new List<MethodDescriptor>();
            Type = type;
        }

        public Type Type { get; }
        public List<PropertyDescriptor> Properties { get; }
        public List<MethodDescriptor> Methods { get; }


        public override int GetHashCode()
            => Type.GetHashCode();

        public override bool Equals(object obj)
            => obj is TypeConfiguration tc && tc.GetHashCode() == GetHashCode();
    }
}
