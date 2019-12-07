using System.Reflection;

namespace RTLang.Interop
{
    // TODO: Better implementation of methodwrapper (type safe delegate from the method info)
    public class MethodWrapper : IMethodWrapper
    {
        private readonly MethodInfo _info;

        public MethodWrapper(MethodInfo info, MethodDescriptor descriptor)
        {
            _info = info;
            Descriptor = descriptor;
        }

        public MethodDescriptor Descriptor { get; }

        public object Execute(object instance, object[] args)
            => _info.Invoke(instance, args);
    }
}
