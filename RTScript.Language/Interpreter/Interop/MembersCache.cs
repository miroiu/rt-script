﻿using System.Collections.Generic;

namespace RTScript.Language.Interpreter.Interop
{
    public class MembersCache
    {
        private readonly Dictionary<PropertyDescriptor, IPropertyWrapper> _properties = new Dictionary<PropertyDescriptor, IPropertyWrapper>();
        private readonly Dictionary<MethodDescriptor, IMethodWrapper> _methods = new Dictionary<MethodDescriptor, IMethodWrapper>();

        public void AddProperty(PropertyDescriptor descriptor, IPropertyWrapper property)
        {
            if (!HasProperty(descriptor))
            {
                _properties.Add(descriptor, property);
            }
        }

        public void AddMethod(MethodDescriptor descriptor, IMethodWrapper method)
        {
            if (!HasMethod(descriptor))
            {
                _methods.Add(descriptor, method);
            }
        }

        public bool TryGetProperty(PropertyDescriptor descriptor, out IPropertyWrapper property)
            => _properties.TryGetValue(descriptor, out property);

        public bool TryGetMethod(MethodDescriptor descriptor, out IMethodWrapper method)
            => _methods.TryGetValue(descriptor, out method);

        public bool HasProperty(PropertyDescriptor descriptor)
            => _properties.ContainsKey(descriptor);

        public bool HasMethod(MethodDescriptor descriptor)
            => _methods.ContainsKey(descriptor);

        public IReadOnlyCollection<IPropertyWrapper> GetProperties()
            => _properties.Values;

        public IReadOnlyCollection<IMethodWrapper> GetMethods()
            => _methods.Values;
    }
}
