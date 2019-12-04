using RTScript.Expressions;
using RTScript.Interpreter.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTScript.Interpreter
{
    public class RTScriptInterpreter
    {
        private readonly IOutputStream _output;
        public IExecutionContext Context { get; }

        public static readonly IDictionary<Type, IExpressionEvaluator> Evaluators = typeof(RTScriptInterpreter).Assembly.GetTypes()
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

        public RTScriptInterpreter(IOutputStream output)
        {
            _output = output;
            Context = new ExecutionContext(_output);

            Context.Declare("int", 0, true);
            Context.Declare("float", 0.0f, true);
            Context.Declare("double", 0.0, true);
            Context.Declare("bool", true, true);
            Context.Declare("char", ' ', true);
            Context.Declare("string", string.Empty, true);
        }

        public void Run(IExpressionProvider provider)
        {
            while (provider.HasNext)
            {
                var expr = provider.Next();
                if (!(expr is EmptyExpression))
                {
                    Reducer.Reduce(expr, Context);
                }
            }
        }
    }
}
