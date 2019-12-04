using RTScript.Lexer;
using System.Diagnostics;

namespace RTScript.Expressions
{
    [DebuggerDisplay("{GetType().Name}")]
    public abstract class Expression
    {
        public Token Token { get; set; }
    }
}