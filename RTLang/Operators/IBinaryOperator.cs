using System;

namespace RTLang.Operators
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
