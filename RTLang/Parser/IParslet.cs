using RTScript.Expressions;

namespace RTScript.Parser
{
    public interface IParslet
    {
        Expression Accept(RTScriptParser parser);
    }
}
