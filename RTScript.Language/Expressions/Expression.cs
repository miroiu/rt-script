using RTScript.Language.Lexer;
using System.Diagnostics;

namespace RTScript.Language.Expressions
{
    [DebuggerDisplay("{GetType().Name}")]
    public abstract class Expression
    {
        public Token Token { get; set; }
    }
}