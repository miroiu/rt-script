namespace RTScript.Language.Interpreter.Interop
{
    public interface IMethodWrapper
    {
        MethodDescriptor Descriptor { get; }
        object Execute(object instance, params object[] args);
    }
}
