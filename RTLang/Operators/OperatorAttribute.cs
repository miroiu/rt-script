using System;

namespace RTScript
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class OperatorAttribute : Attribute
    {
        public OperatorAttribute(UnaryOperatorType operatorType)
        {
            UnaryOperatorType = operatorType;
            IsUnary = true;
        }

        public OperatorAttribute(BinaryOperatorType operatorType)
        {
            BinaryOperatorType = operatorType;
            IsUnary = false;
        }

        public UnaryOperatorType UnaryOperatorType { get; }
        public BinaryOperatorType BinaryOperatorType { get; }

        public bool IsUnary { get; }
    }
}
