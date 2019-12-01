using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RTScript.Language.Interpreter.Interop
{
    public static class TypesCache
    {
        private static readonly Dictionary<TypeConfiguration, MembersCache> _members = new Dictionary<TypeConfiguration, MembersCache>();

        public static void AddType(TypeConfiguration typeConfig)
        {
            if (!_members.ContainsKey(typeConfig))
            {
                var result = new MembersCache();

                foreach (var descriptor in typeConfig.Properties)
                {
                    var property = typeConfig.Type.GetProperty(descriptor.Name, descriptor.PropertyType);
                    var wrapper = descriptor.IsIndexer ? WrapIndexer(typeConfig.Type, property, descriptor) : WrapProperty(typeConfig.Type, property, descriptor);
                    result.AddProperty(descriptor, wrapper);
                }

                foreach (var descriptor in typeConfig.Methods)
                {
                    var method = typeConfig.Type.GetMethod(descriptor.Name, descriptor.Parameters.ToArray());
                    var wrapper = WrapMethod(typeConfig.Type, method, descriptor);
                    result.AddMethod(descriptor, wrapper);
                }

                _members.Add(typeConfig, result);
            }
        }

        public static bool TryGetProperty(Type type, PropertyDescriptor descriptor, out IPropertyWrapper property)
        {
            property = default;
            if (_members.TryGetValue(new TypeConfiguration(type), out var props))
            {
                return props.TryGetProperty(descriptor, out property);
            }

            return false;
        }

        public static IEnumerable<IPropertyWrapper> GetProperties(Type type)
        {
            if (!_members.TryGetValue(new TypeConfiguration(type), out var props))
            {
                return Enumerable.Empty<IPropertyWrapper>();
            }

            return props.GetProperties();
        }

        public static bool TryGetMethod(Type type, MethodDescriptor descriptor, out IMethodWrapper method)
        {
            method = default;
            if (_members.TryGetValue(new TypeConfiguration(type), out var props))
            {
                return props.TryGetMethod(descriptor, out method);
            }

            return false;
        }

        public static IEnumerable<IMethodWrapper> GetMethods(Type type)
        {
            if (!_members.TryGetValue(new TypeConfiguration(type), out var props))
            {
                return Enumerable.Empty<IMethodWrapper>();
            }

            return props.GetMethods();
        }

        #region Wrappers

        // TODO: Better implementation of methodwrapper
        private static IMethodWrapper WrapMethod(Type type, MethodInfo method, MethodDescriptor descriptor)
            => new MethodWrapper(method, descriptor);

        private static IPropertyWrapper WrapIndexer(Type type, PropertyInfo property, PropertyDescriptor descriptor)
        {
            Delegate getterInvocation = default;
            Delegate setterInvocation = default;

            if (descriptor.CanRead)
            {
                MethodInfo getMethod = property.GetGetMethod(true);
                Type getterType = typeof(Func<,,>).MakeGenericType(type, descriptor.ParameterType, descriptor.PropertyType);
                getterInvocation = Delegate.CreateDelegate(getterType, getMethod);
            }

            if (descriptor.CanWrite)
            {
                MethodInfo setMethod = property.GetSetMethod(true);
                Type setterType = typeof(Action<,,>).MakeGenericType(type, descriptor.ParameterType, descriptor.PropertyType);
                setterInvocation = Delegate.CreateDelegate(setterType, setMethod);
            }

            Type adapterType = typeof(IndexerWrapper<,,>).MakeGenericType(type, descriptor.PropertyType, descriptor.ParameterType);

            return (IPropertyWrapper)Activator.CreateInstance(adapterType, getterInvocation, setterInvocation, descriptor);
        }

        private static IPropertyWrapper WrapProperty(Type type, PropertyInfo property, PropertyDescriptor descriptor)
        {
            Delegate getterInvocation = default;
            Delegate setterInvocation = default;

            if (descriptor.CanRead)
            {
                MethodInfo getMethod = property.GetGetMethod(true);
                Type getterType = typeof(Func<,>).MakeGenericType(type, descriptor.PropertyType);
                getterInvocation = Delegate.CreateDelegate(getterType, getMethod);
            }

            if (descriptor.CanWrite)
            {
                MethodInfo setMethod = property.GetSetMethod(true);
                Type setterType = typeof(Action<,>).MakeGenericType(type, descriptor.PropertyType);
                setterInvocation = Delegate.CreateDelegate(setterType, setMethod);
            }

            Type adapterType = typeof(PropertyWrapper<,>).MakeGenericType(type, descriptor.PropertyType);

            return (IPropertyWrapper)Activator.CreateInstance(adapterType, getterInvocation, setterInvocation, descriptor);
        }

        #endregion
    }
}
