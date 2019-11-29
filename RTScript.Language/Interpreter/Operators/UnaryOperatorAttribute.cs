using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter.Operators
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class UnaryOperatorAttribute : Attribute
    {
        public UnaryOperatorAttribute(UnaryOperatorType operatorType)
        {
            OperatorType = operatorType;
        }

        public UnaryOperatorType OperatorType { get; }
    }
}
