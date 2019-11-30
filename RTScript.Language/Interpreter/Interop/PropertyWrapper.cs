using System;

namespace RTScript.Language.Interpreter.Interop
{
    public sealed class PropertyWrapper<TInstanceType, TPropertyType> : IPropertyWrapper
        where TInstanceType : class
    {
        private readonly Func<TInstanceType, TPropertyType> _getterInvocation;
        private readonly Action<TInstanceType, TPropertyType> _setterInvocation;

        public string Name { get; }
        public Type Type { get; }
        public PropertyFlag Flag { get; }

        public PropertyWrapper(Func<TInstanceType, TPropertyType> getterInvocation, Action<TInstanceType, TPropertyType> setterInvocation, string name, Type type)
        {
            _getterInvocation = getterInvocation;
            _setterInvocation = setterInvocation;

            Name = name;
            Type = type;

            Flag = _getterInvocation == default ? PropertyFlag.Write : _setterInvocation == default ? PropertyFlag.Read : PropertyFlag.ReadWrite;
        }

        public object GetValue(object instance, object index = default)
            => _getterInvocation.Invoke((TInstanceType)instance);

        public void SetValue(object instance, object value, object index = default)
            => _setterInvocation.Invoke((TInstanceType)instance, (TPropertyType)value);
    }
}
