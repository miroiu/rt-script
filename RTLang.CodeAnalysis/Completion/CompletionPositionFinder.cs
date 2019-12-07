using RTLang.Lexer;
using RTLang.Parser;

namespace RTLang.CodeAnalysis
{
    internal class CompletionPositionFinder : ILangVisitor<Expression>
    {
        public int Position { get; private set; }

        public void Visit(Expression host)
        {
            var token = host.Token;
            if (token.Type != TokenType.EndOfCode)
            {
                var endOfToken = token.Position + token.Text.Length;

                if (endOfToken > Position)
                {
                    Position = endOfToken;
                }

                host.Accept(this);
            }
            else
            {
                Position = token.Position;
            }
        }

        public bool FindPosition(Expression host, int desiredPosition)
        {
            Visit(host);

            return Position == desiredPosition;
        }
    }
}
