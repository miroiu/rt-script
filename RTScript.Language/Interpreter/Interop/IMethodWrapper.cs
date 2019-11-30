namespace RTScript.Language.Interpreter.Interop
{
    public interface IMethodWrapper
    {
        object Execute(object instance, params object[] args);
    }
}
