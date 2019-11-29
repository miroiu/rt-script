using RTScript.Language.Expressions;

namespace RTScript.Language.Interpreter.Operators
{
    public static class NumberOperators
    {
        #region Unary & Binary

        [BinaryOperator(BinaryOperatorType.Plus)]
        public static double Add(double x, double y) => x + y;

        [BinaryOperator(BinaryOperatorType.Minus)]
        public static double Subtract(double x, double y) => x - y;

        [UnaryOperator(UnaryOperatorType.Minus)]
        public static double Negative(double x) => -x;

        #endregion
    }
}
