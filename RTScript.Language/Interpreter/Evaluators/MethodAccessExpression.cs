using RTScript.Language.Expressions;
using RTScript.Language.Interop;
using System.Collections.Generic;

namespace RTScript.Language.Interpreter.Evaluators
{
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
