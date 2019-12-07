using RTLang.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RTLang
{
    public static partial class TypeHelper
    {
        private static readonly Dictionary<TypeConfiguration, MembersCache> _members = new Dictionary<TypeConfiguration, MembersCache>();

        public static void ConfigureType(TypeConfiguration typeConfig)
        {
            if (!_members.ContainsKey(typeConfig))
            {
                var result = new MembersCache();
                var type = typeConfig.Type;

                foreach (var descriptor in typeConfig.Properties)
                {
                    if (descriptor.IsStatic)
                    {
                        var property = type.GetProperty(descriptor.Name, BindingFlags.Public | BindingFlags.Static);
                        var wrapper = WrapStaticProperty(type, property, descriptor);
                        result.AddProperty(descriptor, wrapper);
                    }
                    else if (descriptor.IsIndexer)
                    {
                        IPropertyWrapper wrapper;

                        if (type.IsArray)
                        {
                            var getter = type.GetMethod("GetValue", new Type[] { typeof(int) });
                            var setter = type.GetMethod("SetValue", new Type[] { typeof(object), typeof(int) });

                            wrapper = WrapIndexer(type, getter, setter, descriptor);
                        }
                        else
                        {
                            var property = type.GetProperty(descriptor.Name, descriptor.ReturnType, new Type[] { descriptor.ParameterType });
                            wrapper = WrapIndexer(type, property, descriptor);
                        }

                        result.AddProperty(descriptor, wrapper);
                    }
                    else
                    {
                        var property = type.GetProperty(descriptor.Name, descriptor.ReturnType);
                        var wrapper = WrapProperty(type, property, descriptor);
                        result.AddProperty(descriptor, wrapper);
                    }
                }

                foreach (var descriptor in typeConfig.Methods)
                {
                    var method = type.GetMethod(descriptor.Name, descriptor.Parameters.ToArray());
                    var wrapper = WrapMethod(type, method, descriptor);
                    result.AddMethod(descriptor, wrapper);
                }

                _members.Add(typeConfig, result);
            }
        }

        public static IEnumerable<IPropertyWrapper> GetProperties(Type type)
        {
            if (!_members.TryGetValue(new TypeConfiguration(type), out var props))
            {
                var config = TypeConfiguration.Build(type);
                ConfigureType(config);
                return _members[config].GetProperties();
            }

            return props.GetProperties();
        }

        public static IEnumerable<IMethodWrapper> GetMethods(Type type)
        {
            if (!_members.TryGetValue(new TypeConfiguration(type), out var props))
            {
                var config = TypeConfiguration.Build(type);
                ConfigureType(config);
                return _members[config].GetMethods();
            }

            return props.GetMethods();
        }

        #region Wrappers

        // TODO: Better implementation of methodwrapper
        private static IMethodWrapper WrapMethod(Type type, MethodInfo method, MethodDescriptor descriptor)
            => new MethodWrapper(method, descriptor);

        private static IPropertyWrapper WrapIndexer(Type type, MethodInfo getter, MethodInfo setter, PropertyDescriptor descriptor)
        {
            Delegate getterInvocation = default;
            Delegate setterInvocation = default;

            if (descriptor.CanRead)
            {
                Type getterType = typeof(Func<,,>).MakeGenericType(type, typeof(int), typeof(object));
                getterInvocation = Delegate.CreateDelegate(getterType, getter);
            }

            if (descriptor.CanWrite)
            {
                Type setterType = typeof(Action<,,>).MakeGenericType(type, typeof(object), typeof(int));
                setterInvocation = Delegate.CreateDelegate(setterType, setter);
            }

            Type adapterType = typeof(IndexerWrapper<>).MakeGenericType(type);

            return (IPropertyWrapper)Activator.CreateInstance(adapterType, getterInvocation, setterInvocation, descriptor);
        }

        private static IPropertyWrapper WrapIndexer(Type type, PropertyInfo property, PropertyDescriptor descriptor)
        {
            Delegate getterInvocation = default;
            Delegate setterInvocation = default;

            if (descriptor.CanRead)
            {
                MethodInfo getMethod = property.GetGetMethod(true);
                Type getterType = typeof(Func<,,>).MakeGenericType(type, descriptor.ParameterType, descriptor.ReturnType);
                getterInvocation = Delegate.CreateDelegate(getterType, getMethod);
            }

            if (descriptor.CanWrite)
            {
                MethodInfo setMethod = property.GetSetMethod(true);
                Type setterType = typeof(Action<,,>).MakeGenericType(type, descriptor.ParameterType, descriptor.ReturnType);
                setterInvocation = Delegate.CreateDelegate(setterType, setMethod);
            }

            Type adapterType = typeof(IndexerWrapper<,,>).MakeGenericType(type, descriptor.ReturnType, descriptor.ParameterType);

            return (IPropertyWrapper)Activator.CreateInstance(adapterType, getterInvocation, setterInvocation, descriptor);
        }

        private static IPropertyWrapper WrapProperty(Type type, PropertyInfo property, PropertyDescriptor descriptor)
        {
            Delegate getterInvocation = default;
            Delegate setterInvocation = default;

            if (descriptor.CanRead)
            {
                MethodInfo getMethod = property.GetGetMethod();
                Type getterType = typeof(Func<,>).MakeGenericType(type, descriptor.ReturnType);
                getterInvocation = Delegate.CreateDelegate(getterType, getMethod);
            }

            if (descriptor.CanWrite)
            {
                MethodInfo setMethod = property.GetSetMethod();
                Type setterType = typeof(Action<,>).MakeGenericType(type, descriptor.ReturnType);
                setterInvocation = Delegate.CreateDelegate(setterType, setMethod);
            }

            Type adapterType = typeof(PropertyWrapper<,>).MakeGenericType(type, descriptor.ReturnType);

            return (IPropertyWrapper)Activator.CreateInstance(adapterType, getterInvocation, setterInvocation, descriptor);
        }

        private static IPropertyWrapper WrapStaticProperty(Type type, PropertyInfo property, PropertyDescriptor descriptor)
        {
            Delegate getterInvocation = default;
            Delegate setterInvocation = default;

            if (descriptor.CanRead)
            {
                MethodInfo getMethod = property.GetGetMethod();
                Type getterType = typeof(Func<>).MakeGenericType(descriptor.ReturnType);
                getterInvocation = Delegate.CreateDelegate(getterType, getMethod);
            }

            if (descriptor.CanWrite)
            {
                MethodInfo setMethod = property.GetSetMethod();
                Type setterType = typeof(Action<>).MakeGenericType(descriptor.ReturnType);
                setterInvocation = Delegate.CreateDelegate(setterType, setMethod);
            }

            Type adapterType = typeof(PropertyWrapper<>).MakeGenericType(descriptor.ReturnType);

            return (IPropertyWrapper)Activator.CreateInstance(adapterType, getterInvocation, setterInvocation, descriptor);
        }

        #endregion
    }
}
