using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RTScript.Language.Interpreter.Interop
{
    // TODO: Make this configurable to wrap only certain properties (marked with attributes or manually added)
    public static class TypeCache
    {
        private static readonly Dictionary<Type, Dictionary<string, IPropertyWrapper>> _instances = new Dictionary<Type, Dictionary<string, IPropertyWrapper>>();

        public static IEnumerable<IPropertyWrapper> GetProperties(Type type)
        {
            if (!_instances.TryGetValue(type, out var props))
            {
                props = new Dictionary<string, IPropertyWrapper>();

                foreach (var property in type.GetProperties())
                {
                    // Only wrap the first indexer found (will remove this check and put a constraint to only allow one indexer in the type configuration builder)
                    if (!props.ContainsKey(property.Name))
                    {
                        var indexParams = property.GetIndexParameters();

                        var propertyWrapper = indexParams.Length == 1 ? WrapIndexer(type, property, indexParams[0]) : WrapProperty(type, property);

                        props.Add(property.Name, propertyWrapper);
                    }
                }

                _instances.Add(type, props);
            }

            return props.Values;
        }

        private static IPropertyWrapper WrapIndexer(Type type, PropertyInfo property, ParameterInfo index)
        {
            MethodInfo getMethod = property.GetGetMethod(true);
            MethodInfo setMethod = property.GetSetMethod(true);

            Delegate getterInvocation = default;
            Delegate setterInvocation = default;

            if (getMethod != null)
            {
                Type getterType = typeof(Func<,,>).MakeGenericType(type, index.ParameterType, property.PropertyType);
                getterInvocation = Delegate.CreateDelegate(getterType, getMethod);
            }

            if (setMethod != null)
            {
                Type setterType = typeof(Action<,,>).MakeGenericType(type, index.ParameterType, property.PropertyType);
                setterInvocation = Delegate.CreateDelegate(setterType, setMethod);
            }

            Type adapterType = typeof(IndexerWrapper<,,>).MakeGenericType(type, property.PropertyType, index.ParameterType);

            return (IPropertyWrapper)Activator.CreateInstance(adapterType, getterInvocation, setterInvocation, property.Name, property.PropertyType);
        }

        private static IPropertyWrapper WrapProperty(Type type, PropertyInfo property)
        {
            MethodInfo getMethod = property.GetGetMethod(true);
            MethodInfo setMethod = property.GetSetMethod(true);

            Delegate getterInvocation = default;
            Delegate setterInvocation = default;

            if (getMethod != null)
            {
                Type getterType = typeof(Func<,>).MakeGenericType(type, property.PropertyType);
                getterInvocation = Delegate.CreateDelegate(getterType, getMethod);
            }

            if (setMethod != null)
            {
                Type setterType = typeof(Action<,>).MakeGenericType(type, property.PropertyType);
                setterInvocation = Delegate.CreateDelegate(setterType, setMethod);
            }

            Type adapterType = typeof(PropertyWrapper<,>).MakeGenericType(type, property.PropertyType);

            return (IPropertyWrapper)Activator.CreateInstance(adapterType, getterInvocation, setterInvocation, property.Name, property.PropertyType);
        }

        public static IPropertyWrapper GetProperty(Type instanceType, string name)
        {
            return GetProperties(instanceType).FirstOrDefault(p => p.Name == name);
        }
    }
}
