namespace RTLang.Expressions
{
    public interface IExpressionProvider
    {
        bool HasNext { get; }
        Expression Next();
    }
}