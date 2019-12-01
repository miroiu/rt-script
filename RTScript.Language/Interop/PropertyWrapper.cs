using System;

namespace RTScript.Language.Interop
{
    public sealed class PropertyWrapper<TInstanceType, TPropertyType> : IPropertyWrapper
        where TInstanceType : class
    {
        private readonly Func<TInstanceType, TPropertyType> _getterInvocation;
        private readonly Action<TInstanceType, TPropertyType> _setterInvocation;

        public PropertyWrapper(Func<TInstanceType, TPropertyType> getterInvocation, Action<TInstanceType, TPropertyType> setterInvocation, PropertyDescriptor descriptor)
        {
            Descriptor = descriptor;

            _getterInvocation = getterInvocation;
            _setterInvocation = setterInvocation;
        }

        public PropertyDescriptor Descriptor { get; }

        public object GetValue(object instance, object index = default)
            => _getterInvocation.Invoke((TInstanceType)instance);

        public void SetValue(object instance, object value, object index = default)
            => _setterInvocation.Invoke((TInstanceType)instance, (TPropertyType)value);
    }
}
