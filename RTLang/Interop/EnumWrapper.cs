using System;

namespace RTLang.Interop
{
    // Enum field
    public sealed class EnumWrapper<TEnumType> : IPropertyWrapper
    {
        public EnumWrapper(PropertyDescriptor descriptor)
            => Descriptor = descriptor;

        public PropertyDescriptor Descriptor { get; }

        public object GetValue(object instance, object index = default)
            => Enum.Parse(typeof(TEnumType), Descriptor.Name);

        public void SetValue(object instance, object value, object index = default)
        {

        }
    }
}
