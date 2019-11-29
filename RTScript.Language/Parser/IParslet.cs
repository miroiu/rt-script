using RTScript.Language.Expressions;

namespace RTScript.Language.Parser
{
    public interface IParslet
    {
        Expression Accept(RTScriptParser parser);
    }
}
