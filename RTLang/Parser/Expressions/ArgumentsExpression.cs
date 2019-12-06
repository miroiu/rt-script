using System.Collections.Generic;

namespace RTLang.Parser
{
    public class ArgumentsExpression : Expression
    {
        public ArgumentsExpression(IReadOnlyList<Expression> arguments)
            => Items = arguments;

        public IReadOnlyList<Expression> Items { get; }

        public override void Accept(ILangVisitor<Expression> visitor)
        {
            foreach (var arg in Items)
            {
                visitor.Visit(arg);
            }
        }
    }
}
