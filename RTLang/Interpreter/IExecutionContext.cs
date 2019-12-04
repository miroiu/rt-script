using System;

namespace RTLang
{
    public interface IExecutionContext
    {
        void Assign(string name, object value);
        void Declare(string name, object value, bool isConst = false);
        object GetValue(string name);
        Type GetType(string name);
        void Print(object value);
        object Evaluate(LiteralType type, string value);
        object Evaluate(UnaryOperatorType operatorType, object value);
        object Evaluate(BinaryOperatorType operatorType, object left, object right);
    }
}