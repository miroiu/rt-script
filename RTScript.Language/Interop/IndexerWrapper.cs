using System;

namespace RTScript.Language.Interop
{
    public sealed class IndexerWrapper<TInstanceType, TPropertyType, TIndexType> : IPropertyWrapper
        where TInstanceType : class
    {
        private readonly Func<TInstanceType, TIndexType, TPropertyType> _getterInvocation;
        private readonly Action<TInstanceType, TIndexType, TPropertyType> _setterInvocation;

        public PropertyDescriptor Descriptor { get; }

        public IndexerWrapper(Func<TInstanceType, TIndexType, TPropertyType> getterInvocation, Action<TInstanceType, TIndexType, TPropertyType> setterInvocation, PropertyDescriptor descriptor)
        {
            _getterInvocation = getterInvocation;
            _setterInvocation = setterInvocation;

            Descriptor = descriptor;
        }

        public object GetValue(object instance, object index = default)
            => _getterInvocation.Invoke((TInstanceType)instance, (TIndexType)index);

        public void SetValue(object instance, object value, object index = default)
            => _setterInvocation.Invoke((TInstanceType)instance, (TIndexType)index, (TPropertyType)value);
    }
}
