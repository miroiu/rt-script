using RTLang.Parser;

namespace RTLang.CodeCompletion.Parser
{
    public class EmptyParslet : IParslet
    {
        public Expression Accept(IRTLangParser parser)
            => new EmptyExpression();
    }
}
