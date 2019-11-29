using RTScript.Language.Expressions;

namespace RTScript.Language
{
    public interface IExpressionProvider
    {
        bool HasNext { get; }
        Expression Next();
    }
}