namespace RTLang.Interop
{
    public interface IPropertyWrapper
    {
        PropertyDescriptor Descriptor { get; }
        object GetValue(object instance, object index = default);
        void SetValue(object instance, object value, object index = default);
    }
}
