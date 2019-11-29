using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter.Operators
{
    public interface IBinaryOperator
    {
        object Execute(object left, object right);
        Type LeftType { get; }
        Type RightType { get; }
        Type ReturnType { get; }
        BinaryOperatorType OperatorType { get; }
    }
}
