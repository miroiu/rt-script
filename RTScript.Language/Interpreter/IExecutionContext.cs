using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter
{
    public interface IExecutionContext
    {
        void Assign(string name, object value);
        void Declare(string name, object value);
        object Get(string name);
        void Print(object value);
        object Evaluate(LiteralType type, string value);
        object Evaluate(UnaryOperatorType operatorType, object value);
        object Evaluate(object left, BinaryOperatorType operatorType, object right);
    }
}