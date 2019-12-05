namespace RTLang.Parser
{
    public interface IExpressionProvider
    {
        bool HasNext { get; }
        Expression Next();
    }
}