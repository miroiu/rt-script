using System;
using System.Collections.Generic;

namespace RTLang.Interop
{
    public class MembersCache
    {
        private readonly Dictionary<DescriptorType, Dictionary<PropertyDescriptor, IPropertyWrapper>> _properties = new Dictionary<DescriptorType, Dictionary<PropertyDescriptor, IPropertyWrapper>>()
        {
            [DescriptorType.Enum] = new Dictionary<PropertyDescriptor, IPropertyWrapper>(),
            [DescriptorType.Indexer] = new Dictionary<PropertyDescriptor, IPropertyWrapper>(),
            [DescriptorType.Property] = new Dictionary<PropertyDescriptor, IPropertyWrapper>(),
        };

        private readonly Dictionary<MethodDescriptor, IMethodWrapper> _methods = new Dictionary<MethodDescriptor, IMethodWrapper>();

        public void AddProperty(PropertyDescriptor descriptor, IPropertyWrapper property)
        {
            var props = _properties[descriptor.DescriptorType];
            if (!props.ContainsKey(descriptor))
            {
                props.Add(descriptor, property);
            }
        }

        public void AddMethod(MethodDescriptor descriptor, IMethodWrapper method)
        {
            if (!HasMethod(descriptor))
            {
                _methods.Add(descriptor, method);
            }
        }

        public bool HasMethod(MethodDescriptor descriptor)
            => _methods.ContainsKey(descriptor);

        public IReadOnlyCollection<IPropertyWrapper> GetProperties(DescriptorType type)
        {
            if (_properties.TryGetValue(type, out var result))
            {
                return result.Values;
            }

            return Array.Empty<IPropertyWrapper>();
        }

        public IReadOnlyCollection<IMethodWrapper> GetMethods()
            => _methods.Values;
    }
}
