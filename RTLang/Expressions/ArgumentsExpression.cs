using System.Collections.Generic;

namespace RTLang.Expressions
{
    public class ArgumentsExpression : Expression
    {
        public ArgumentsExpression(IReadOnlyList<Expression> arguments)
            => Items = arguments;

        public IReadOnlyList<Expression> Items { get; }
    }
}
