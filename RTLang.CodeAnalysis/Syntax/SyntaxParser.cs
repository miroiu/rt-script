using RTLang.Lexer;
using RTLang.Parser;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis.Syntax
{
    internal class SyntaxParser : IExpressionProvider, IRTLangParser
    {
        public static readonly IDictionary<(TokenType Type, bool CanBeginWith), IParslet> Parslets = Parser.Parser.Parslets;
        private readonly Lexer.Lexer _lexer;

        public bool HasNext { get; private set; }
        public Token Current { get; private set; }
        public bool ThrowOnError { get; }

        public SyntaxParser(Lexer.Lexer lexer, bool throwOnError)
        {
            _lexer = lexer;
            ThrowOnError = throwOnError;
            HasNext = Peek().Type != TokenType.EndOfCode;
        }

        #region API

        public Expression Next()
        {
            Current = _lexer.Lex();
            var result = GetParslet(Current, true).Accept(this);

            if (ThrowOnError)
            {
                Ensure(TokenType.Semicolon);
            }

            HasNext = Peek().Type != TokenType.EndOfCode;

            return result;
        }

        public Token Match(TokenType tokenType)
        {
            Ensure(tokenType);
            return Take();
        }

        public Token Peek(int offset = 1)
            => _lexer.Peek(offset);

        public Token Take()
        {
            var previous = Current;
            Current = _lexer.Lex();
            return previous;
        }

        public void Ensure(TokenType type)
        {
            if ((ThrowOnError && Current.Type != type) || Current.Type == TokenType.EndOfCode)
            {
                // Missing mandatory token should raise a diagnostic
                throw new SyntaxException(new Token
                {
                    Line = Current.Line,
                    Column = Current.Column,
                    Position = Current.Position,
                    Type = type,
                    Text = type.ToFriendlyString()
                }, Current);
            }
        }

        public bool IsEndOfStatement()
            => Current.Type == TokenType.Semicolon;

        private IParslet GetParslet(Token token, bool canBeginWith)
        {
            if (Parslets.TryGetValue((token.Type, canBeginWith), out var value))
            {
                return value;
            }

            if (ThrowOnError)
            {
                throw new SyntaxException(token, $"Invalid token found: {token.Text}");
            }

            return new EmptyParslet();
        }

        #endregion

        #region Parsers

        public Expression ParseExpression()
            => Parslets[(TokenType.Identifier, true)].Accept(this);

        public Expression ParsePrimaryExpression()
            => GetParslet(Current, false).Accept(this);

        public ArgumentsExpression ParseArguments(TokenType closingToken)
        {
            List<Expression> arguments = new List<Expression>();

            while (!IsEndOfStatement() && Current.Type != closingToken)
            {
                var arg = ParseExpression();

                if (Current.Type != closingToken)
                {
                    Match(TokenType.Comma);
                }

                arguments.Add(arg);
            }

            return new ArgumentsExpression(arguments)
            {
                Token = Current
            };
        }

        #endregion
    }
}
