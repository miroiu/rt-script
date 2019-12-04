using System;

namespace RTScript
{
    public interface IUnaryOperator
    {
        object Execute(object value);
        Type ParameterType { get; }
        Type ReturnType { get; }
        UnaryOperatorType OperatorType { get; }
    }
}
