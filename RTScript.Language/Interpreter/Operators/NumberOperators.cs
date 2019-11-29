using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Operators
{
    public static class NumberOperators
    {
        #region Math

        [BinaryOperator(BinaryOperatorType.Plus)]
        public static double Add(double x, double y) => x + y;

        [BinaryOperator(BinaryOperatorType.Minus)]
        public static double Subtract(double x, double y) => x - y;

        [BinaryOperator(BinaryOperatorType.Multiply)]
        public static double Multiply(double x, double y) => x * y;

        [BinaryOperator(BinaryOperatorType.Divide)]
        public static double Divide(double x, double y) => x / y;

        [UnaryOperator(UnaryOperatorType.Minus)]
        public static double Negative(double x) => -x;

        #endregion

        #region Logical

        [BinaryOperator(BinaryOperatorType.Greater)]
        public static bool Greater(double x, double y) => x > y;

        [BinaryOperator(BinaryOperatorType.Less)]
        public static bool Less(double x, double y) => x < y;

        [BinaryOperator(BinaryOperatorType.Equal)]
        public static bool Equal(double x, double y) => x == y;

        [BinaryOperator(BinaryOperatorType.NotEqual)]
        public static bool NotEqual(double x, double y) => x != y;

        #endregion
    }
}
