using RTLang.Interpreter;
using RTLang.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeCompletion.Evaluator
{
    internal class Reducer : ILangVisitor<Expression>
    {
        internal static readonly IDictionary<Type, ICompletionEvaluator> Evaluators = typeof(ICompletionEvaluator).Assembly.GetTypes()
                .Where(x => typeof(ICompletionEvaluator).IsAssignableFrom(x) && x.CustomAttributes.Any())
                .SelectMany(x =>
                {
                    return x.GetCustomAttributes(false).Select(y => new
                    {
                        Attribute = (ExpressionEvaluatorAttribute)y,
                        Type = x
                    }).ToList();
                })
                .ToDictionary(x => x.Attribute.ExpressionType, x => Activator.CreateInstance(x.Type) as ICompletionEvaluator);

        private readonly EvaluationBag _bag;

        public Reducer(EvaluationBag bag)
        {
            _bag = bag;
        }

        public void Visit(Expression host)
        {
            bool isRequestingCompletion = host.Token.Column + host.Token.Text.Length == _bag.CompletionPosition;

            if (Evaluators.TryGetValue(host.GetType(), out var evaluator))
            {
                evaluator.Evaluate(host, _bag, isRequestingCompletion);
            }

            host.Accept(this);
        }
    }
}
