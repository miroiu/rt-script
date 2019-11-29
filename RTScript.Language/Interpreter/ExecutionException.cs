using RTScript.Language.Expressions;
using System;

namespace RTScript.Language.Interpreter
{
    public class ExecutionException : RTScriptException
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
