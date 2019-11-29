using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter.Operators
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BinaryOperatorAttribute : Attribute
    {
        public BinaryOperatorAttribute(BinaryOperatorType operatorType)
        {
            OperatorType = operatorType;
        }

        public BinaryOperatorType OperatorType { get; }
    }
}
