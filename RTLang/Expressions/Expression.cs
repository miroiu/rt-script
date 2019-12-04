using RTLang.Lexer;
using System.Diagnostics;

namespace RTLang.Expressions
{
    [DebuggerDisplay("{GetType().Name}")]
    public abstract class Expression
    {
        public Token Token { get; set; }
    }
}