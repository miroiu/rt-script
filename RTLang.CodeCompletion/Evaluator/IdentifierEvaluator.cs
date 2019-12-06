using RTLang.Interpreter;
using RTLang.Parser;
using System.Linq;

namespace RTLang.CodeCompletion.Evaluator
{
    [ExpressionEvaluator(typeof(IdentifierExpression))]
    public class IdentifierEvaluator : ICompletionEvaluator
    {
        public void Evaluate(Expression expression, EvaluationBag bag, bool isRequestingCompletion)
        {
            var casted = (IdentifierExpression)expression;
            var words = bag.Context.GetSymbols().Where(s => s.StartsWith(casted.Name) && !(s == casted.Name));

            if (isRequestingCompletion)
            {
                bag.Completions.AddRange(words.Select(w => new Completion
                {
                    Text = w
                }).ToList());
            }
            else
            {
                if (!words.Contains(casted.Name))
                {
                    bag.Errors.Add(new EvaluationError
                    {
                        Position = casted.Token.Column,
                        Length = casted.Token.Text.Length,
                        Info = $"'{casted.Name}' is not defined in the current context."
                    });
                }
            }
        }
    }
}