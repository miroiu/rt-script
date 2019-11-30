using System.Reflection;

namespace RTScript.Language.Interpreter.Interop
{
    // TODO: Try to create a type safe delegate from the method info (using Linq.Expressions?) like it is done for the operators
    public class MethodWrapper<TResult> : IMethodWrapper
    {
        private readonly MethodInfo _info;

        public MethodWrapper(MethodInfo info)
            => _info = info;

        public object Execute(object instance, params object[] args)
            => _info.Invoke(instance, args);
    }
}
