namespace RTScript.Language.Interop
{
    public interface IMethodWrapper
    {
        MethodDescriptor Descriptor { get; }
        object Execute(object instance, object[] args);
    }
}
