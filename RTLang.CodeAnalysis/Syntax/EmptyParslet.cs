using RTLang.Parser;

namespace RTLang.CodeAnalysis.Syntax
{
    internal class EmptyParslet : IParslet
    {
        public Expression Accept(IRTLangParser parser)
            => new EmptyExpression();
    }
}
