using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Operators
{
    public static class NumberOperators
    {
        #region Math

        [Operator(BinaryOperatorType.Plus)]
        public static double Add(double x, double y) => x + y;

        [Operator(BinaryOperatorType.Minus)]
        public static double Subtract(double x, double y) => x - y;

        [Operator(BinaryOperatorType.Multiply)]
        public static double Multiply(double x, double y) => x * y;

        [Operator(BinaryOperatorType.Divide)]
        public static double Divide(double x, double y) => x / y;

        [Operator(UnaryOperatorType.Minus)]
        public static double Negative(double x) => -x;

        #endregion

        #region Logical

        [Operator(BinaryOperatorType.Greater)]
        public static bool Greater(double x, double y) => x > y;

        [Operator(BinaryOperatorType.Less)]
        public static bool Less(double x, double y) => x < y;

        [Operator(BinaryOperatorType.Equal)]
        public static bool Equal(double x, double y) => x == y;

        [Operator(BinaryOperatorType.NotEqual)]
        public static bool NotEqual(double x, double y) => x != y;

        #endregion
    }
}
