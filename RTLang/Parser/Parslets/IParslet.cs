namespace RTLang.Parser
{
    public interface IParslet
    {
        Expression Accept(IRTLangParser parser);
    }
}
