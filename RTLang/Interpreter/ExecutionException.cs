using RTLang.Expressions;
using System;

namespace RTLang.Interpreter
{
    public class ExecutionException : RTLangException
    {
        public Expression Expression { get; private set; }
        public ExecutionException(string message, Expression e) : base(message)
        {
            Expression = e;
        }

        public ExecutionException(Exception ex, Expression e) : base(ex.Message)
        {
            Expression = e;
        }
    }
}
