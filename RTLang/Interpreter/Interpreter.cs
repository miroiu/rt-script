using RTLang.Expressions;

namespace RTLang.Interpreter
{
    public sealed class Interpreter
    {
        private readonly IExpressionProvider _expressionProvider;

        public Interpreter(IExpressionProvider expressionProvider)
            => _expressionProvider = expressionProvider;

        public void Run(IExecutionContext context)
        {
            while (_expressionProvider.HasNext)
            {
                var expr = _expressionProvider.Next();
                Reducer.Reduce(expr, context);
            }
        }
    }
}
