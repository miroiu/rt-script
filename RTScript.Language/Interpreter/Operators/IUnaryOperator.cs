using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter.Operators
{
    public interface IUnaryOperator
    {
        object Execute(object value);
        Type ParameterType { get; }
        Type ReturnType { get; }
        UnaryOperatorType OperatorType { get; }
    }
}
