using RTScript.Expressions;

namespace RTScript
{
    public interface IExpressionProvider
    {
        bool HasNext { get; }
        Expression Next();
    }
}