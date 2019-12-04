using System;

namespace RTLang.Operators
{
    public interface IUnaryOperator
    {
        object Execute(object value);
        Type ParameterType { get; }
        Type ReturnType { get; }
        UnaryOperatorType OperatorType { get; }
    }
}
