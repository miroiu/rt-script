using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RTScript.Language.Interop
{
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

        public static TypeConfiguration Build<T>()
            => Build(typeof(T));

        public static TypeConfiguration Build(Type type)
        {
            var config = new TypeConfiguration(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var indexParams = property.GetIndexParameters();
                if (indexParams.Length > 0)
                {
                    var param = indexParams[0];
                    var prop = new PropertyDescriptor(property.Name, property.PropertyType, param.ParameterType, property.CanRead, property.CanWrite, isStatic: false, isIndexer: true);
                    config.Properties.Add(prop);
                }
                else
                {
                    var prop = new PropertyDescriptor(property.Name, property.PropertyType, property.PropertyType, property.CanRead, property.CanWrite, isStatic: false, isIndexer: false);
                    config.Properties.Add(prop);
                }
            }

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                var prop = new PropertyDescriptor(property.Name, property.PropertyType, property.PropertyType, property.CanRead, property.CanWrite, isStatic: true, isIndexer: false);
                config.Properties.Add(prop);
            }

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var med = new MethodDescriptor(method.Name, method.IsStatic, method.ReturnType, parameters);
                config.Methods.Add(med);
            }

            if(type.IsArray)
            {
                var desc = new PropertyDescriptor("Item", typeof(object), typeof(int), true, true, false, true);
                config.Properties.Add(desc);
            }

            return config;
        }
    }
}
