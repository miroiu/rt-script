namespace RTLang.Parser
{
    public class UnaryExpression : Expression
    {
        public UnaryExpression(UnaryOperatorType operatorType, Expression operand)
        {
            OperatorType = operatorType;
            Operand = operand;
        }

        public UnaryOperatorType OperatorType { get; }
        public Expression Operand { get; }

        public override void Accept(ILangVisitor<Expression> visitor)
        {
            visitor.Visit(Operand);
        }
    }
}
