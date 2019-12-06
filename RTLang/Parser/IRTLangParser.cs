using RTLang.Lexer;

namespace RTLang.Parser
{
    public interface IRTLangParser
    {
        Token Current { get; }
        Token Peek(int offset = 1);
        Token Match(TokenType tokenType);
        Token Take();
        void Ensure(TokenType type);
        bool IsEndOfStatement();
        Expression ParseExpression();
        Expression ParsePrimaryExpression();
        ArgumentsExpression ParseArguments(TokenType closingToken);
    }
}
