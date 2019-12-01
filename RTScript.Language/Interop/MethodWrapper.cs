using System.Reflection;

namespace RTScript.Language.Interop
{
    // TODO: Try to create a type safe delegate from the method info (using Linq.Expressions?) like it is done for the operators
    public class MethodWrapper : IMethodWrapper
    {
        private readonly MethodInfo _info;

        public MethodWrapper(MethodInfo info, MethodDescriptor descriptor)
        {
            _info = info;
            Descriptor = descriptor;
        }

        public MethodDescriptor Descriptor { get; }

        public object Execute(object instance, params object[] args)
            => _info.Invoke(instance, args);
    }
}
