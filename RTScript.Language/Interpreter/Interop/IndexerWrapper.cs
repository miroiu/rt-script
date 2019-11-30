using System;

namespace RTScript.Language.Interpreter.Interop
{
    public sealed class IndexerWrapper<TInstanceType, TPropertyType, TIndexType> : IPropertyWrapper
        where TInstanceType : class
    {
        private readonly Func<TInstanceType, TIndexType, TPropertyType> _getterInvocation;
        private readonly Action<TInstanceType, TIndexType, TPropertyType> _setterInvocation;

        public string Name { get; }
        public Type Type { get; }
        public PropertyFlag Flag { get; }

        public IndexerWrapper(Func<TInstanceType, TIndexType, TPropertyType> getterInvocation, Action<TInstanceType, TIndexType, TPropertyType> setterInvocation, string name, Type type)
        {
            _getterInvocation = getterInvocation;
            _setterInvocation = setterInvocation;

            Name = name;
            Type = type;

            Flag = _getterInvocation == default ? PropertyFlag.Write : _setterInvocation == default ? PropertyFlag.Read : PropertyFlag.ReadWrite;
        }

        public object GetValue(object instance, object index = default)
            => _getterInvocation.Invoke((TInstanceType)instance, (TIndexType)index);

        public void SetValue(object instance, object value, object index = default)
            => _setterInvocation.Invoke((TInstanceType)instance, (TIndexType)index, (TPropertyType)value);
    }
}
