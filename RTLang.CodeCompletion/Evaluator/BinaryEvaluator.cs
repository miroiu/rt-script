using RTLang.Interpreter;
using RTLang.Parser;

namespace RTLang.CodeCompletion.Evaluator
{
    [ExpressionEvaluator(typeof(BinaryExpression))]
    public class BinaryEvaluator : ICompletionEvaluator
    {
        public void Evaluate(Expression expression, EvaluationBag bag, bool isRequestingCompletion)
        {
            var casted = (BinaryExpression)expression;

            switch (casted.OperatorType)
            {
                case BinaryOperatorType.AccessMember:
                    var left = casted.Left;
                    var right = casted.Right;

                    break;
            }
        }
    }
}
