using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RTScript.Language.Interpreter.Interop
{
    public static class TypeCache
    {
        private static readonly Dictionary<TypeConfiguration, Dictionary<string, IPropertyWrapper>> _instances = new Dictionary<TypeConfiguration, Dictionary<string, IPropertyWrapper>>();

        public static bool TryGetProperty(Type type, string name, out IPropertyWrapper property)
        {
            property = default;
            if (_instances.TryGetValue(new TypeConfiguration { Type = type }, out var props))
            {
                if (props.TryGetValue(name, out var prop))
                {
                    property = prop;
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<IPropertyWrapper> GetProperties(Type type)
        {
            if (!_instances.TryGetValue(new TypeConfiguration { Type = type }, out var props))
            {
                return Enumerable.Empty<IPropertyWrapper>();
            }

            return props.Values;
        }

        public static void AddType(TypeConfiguration typeConfig)
        {
            if (!_instances.ContainsKey(typeConfig))
            {
                // TODO: Performance test on GetProperty multiple times vs filtered GetProperties
                var properties = typeConfig.IsConfigured ? typeConfig.Properties.Select(p => typeConfig.Type.GetProperty(p)) : typeConfig.Type.GetProperties();
                var result = new Dictionary<string, IPropertyWrapper>();

                foreach (var property in properties)
                {
                    // Allow only one indexer
                    if (!result.ContainsKey(property.Name))
                    {
                        var indexParams = property.GetIndexParameters();
                        var propertyWrapper = indexParams.Length == 1 ? WrapIndexer(typeConfig.Type, property, indexParams[0]) : WrapProperty(typeConfig.Type, property);

                        result.Add(property.Name, propertyWrapper);
                    }
                }

                _instances.Add(typeConfig, result);
            }
        }

        #region Wrappers

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

        #endregion
    }
}
