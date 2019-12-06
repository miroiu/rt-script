using RTLang.Parser;

namespace RTLang.CodeCompletion
{
    public interface ICompletionEvaluator
    {
        void Evaluate(Expression expression, EvaluationBag bag, bool isRequestingCompletion);
    }
}