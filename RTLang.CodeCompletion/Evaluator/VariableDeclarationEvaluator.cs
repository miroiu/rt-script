using RTLang.Interpreter;
using RTLang.Parser;
using System.Linq;

namespace RTLang.CodeCompletion.Evaluator
{
    [ExpressionEvaluator(typeof(VariableDeclarationExpression))]
    public class VariableDeclarationEvaluator : ICompletionEvaluator
    {
        public void Evaluate(Expression expression, EvaluationBag bag, bool isRequestingCompletion)
        {
            var casted = (VariableDeclarationExpression)expression;
            var alreadyExists = bag.Context.GetSymbols().Any(s => s == casted.Identifier);

            if(alreadyExists)
            {
                bag.Errors.Add(new EvaluationError
                {
                    Position = casted.Token.Column,
                    Length = casted.Token.Text.Length,
                    Info = $"'{casted.Identifier}' is already defined in the current context."
                });
            }
        }
    }
}
