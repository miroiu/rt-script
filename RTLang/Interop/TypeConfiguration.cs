﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RTLang.Interop
{
    public class TypeConfiguration
    {
        public TypeConfiguration(Type type)
        {
            Properties = new List<PropertyDescriptor>(4);
            Methods = new List<MethodDescriptor>(8);
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
            if (type == default)
            {
                throw new ArgumentNullException($"{nameof(type)} is null.");
            }

            var config = new TypeConfiguration(type);

            BuildProperties(type, config);
            BuildMethods(type, config);

            return config;
        }

        public static bool TryGetDescriptor(MethodInfo method, out MethodDescriptor descriptor)
        {
            descriptor = default;

            if (!method.ContainsGenericParameters)
            {
                var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
                descriptor = new MethodDescriptor(method.Name, method.IsStatic, method.ReturnType, parameters);
                return true;
            }

            return false;
        }

        private static void BuildMethods(Type type, TypeConfiguration config)
        {
            var allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            for (var i = 0; i < allMethods.Length; i++)
            {
                if (TryGetDescriptor(allMethods[i], out var descriptor))
                {
                    config.Methods.Add(descriptor);
                }
            }
        }

        private static void BuildProperties(Type type, TypeConfiguration config)
        {
            // Instance
            var instanceProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < instanceProperties.Length; i++)
            {
                var property = instanceProperties[i];
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

            // Static
            var staticProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            for (var i = 0; i < staticProperties.Length; i++)
            {
                var property = staticProperties[i];
                var prop = new PropertyDescriptor(property.Name, property.PropertyType, property.PropertyType, property.CanRead, property.CanWrite, isStatic: true, isIndexer: false);
                config.Properties.Add(prop);
            }

            // Array
            if (type.IsArray)
            {
                var desc = new PropertyDescriptor(string.Empty, typeof(object), typeof(int), true, true, false, true);
                config.Properties.Add(desc);
            }
        }
    }
}
