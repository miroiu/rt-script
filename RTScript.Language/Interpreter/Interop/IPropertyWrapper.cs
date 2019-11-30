using System;

namespace RTScript.Language.Interpreter.Interop
{
    public interface IPropertyWrapper
    {
        string Name { get; }
        Type Type { get; }
        PropertyFlag Flag { get; }
        object GetValue(object instance, object index = default);
        void SetValue(object instance, object value, object index = default);
    }
}
