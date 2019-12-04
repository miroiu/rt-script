using RTLang.Expressions;

namespace RTLang.Interpreter
{
    public sealed class Interpreter
    {
        public IExecutionContext Context { get; }

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
