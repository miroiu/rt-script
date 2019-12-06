namespace RTLang.Parser
{
    public class BinaryExpression : Expression
    {
        public BinaryExpression(Expression left, BinaryOperatorType operatorType, Expression right)
        {
            Left = left;
            OperatorType = operatorType;
            Right = right;
        }

        public Expression Left { get; }
        public BinaryOperatorType OperatorType { get; }
        public Expression Right { get; }

        public override void Accept(ILangVisitor<Expression> visitor)
        {
            visitor.Visit(Left);
            visitor.Visit(Right);
        }
    }
}
