using RTLang.Parser;
using System;

namespace RTLang.Interpreter
{
    public class ExecutionException : RTLangException
    {
        public ExecutionException(string message, Expression e) : this(message, e.Token.Line, e.Token.Column)
        {

        }

        public ExecutionException(Exception ex, Expression e) : this(ex.Message, e.Token.Line, e.Token.Column)
        {

        }

        public ExecutionException(string message, int line, int column) : base(message)
        {
            Line = line;
            Column = column;
        }

        public int Line { get; }
        public int Column { get; }
    }
}
