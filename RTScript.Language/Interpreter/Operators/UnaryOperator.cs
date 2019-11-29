using System;
using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Operators
{
    public class UnaryOperator<TValue, TResult> : IUnaryOperator
    {
        private readonly Func<TValue, TResult> _unaryOperator;
        public UnaryOperatorType OperatorType { get; }

        public UnaryOperator(Func<TValue, TResult> unaryOperator, UnaryOperatorType type)
        {
            _unaryOperator = unaryOperator;
            ParameterType = typeof(TValue);
            ReturnType = typeof(TResult);
            OperatorType = type;
        }

        public Type ParameterType { get; }

        public Type ReturnType { get; }

        public object Execute(object value)
            => _unaryOperator((TValue)value);
    }
}
