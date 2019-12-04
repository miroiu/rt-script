using System;

namespace RTScript
{
    public class BinaryOperator<TLeft, TRight, TResult> : IBinaryOperator
    {
        private readonly Func<TLeft, TRight, TResult> _binaryOperator;

        public Type LeftType { get; }

        public Type RightType { get; }

        public Type ReturnType { get; }

        public BinaryOperatorType OperatorType { get; }

        public BinaryOperator(Func<TLeft, TRight, TResult> binaryOperator, BinaryOperatorType type)
        {
            _binaryOperator = binaryOperator;
            LeftType = typeof(TLeft);
            RightType = typeof(TRight);
            ReturnType = typeof(TResult);
            OperatorType = type;
        }

        public object Execute(object left, object right)
            => _binaryOperator((TLeft)left, (TRight)right);
    }
}
