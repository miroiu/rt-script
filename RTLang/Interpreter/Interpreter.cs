using RTLang.Expressions;
using RTLang.Interpreter.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.Interpreter
{
    public sealed class Interpreter
    {
        public IExecutionContext Context { get; }

        public static readonly IDictionary<Type, IExpressionEvaluator> Evaluators = typeof(Interpreter).Assembly.GetTypes()
                .Where(x => typeof(IExpressionEvaluator).IsAssignableFrom(x) && x.CustomAttributes.Any())
                .SelectMany(x =>
                {
                    return x.GetCustomAttributes(false).Select(y => new
                    {
                        Attribute = (ExpressionEvaluatorAttribute)y,
                        Type = x
                    }).ToList();
                })
                .ToDictionary(x => x.Attribute.ExpressionType, x => Activator.CreateInstance(x.Type) as IExpressionEvaluator);

        public Interpreter(IOutputStream output)
            => Context = new ExecutionContext(output);

        public Interpreter(IExecutionContext context)
            => Context = context;

        public void Run(IExpressionProvider provider)
        {
            while (provider.HasNext)
            {
                var expr = provider.Next();
                Reducer.Reduce(expr, Context);
            }
        }
    }
}
