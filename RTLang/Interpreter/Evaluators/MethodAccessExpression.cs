using RTLang.Expressions;
using RTLang.Interop;
using System.Collections.Generic;

namespace RTLang.Interpreter.Evaluators
{
    // Used only be the evaluators
    public class MethodAccessExpression : Expression
    {
        public MethodAccessExpression(object instance, IMethodWrapper method, object[] arguments)
        {
            Instance = instance;
            Method = method;
            Arguments = arguments;
        }

        public object Instance { get; }
        public IMethodWrapper Method { get; }
        public IReadOnlyList<object> Arguments { get; }
    }
}
