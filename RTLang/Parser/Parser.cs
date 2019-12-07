using RTLang.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.Parser
{
    public class Parser : IExpressionProvider, IRTLangParser
    {
        public static readonly IDictionary<(TokenType Type, bool CanBeginWith), IParslet> Parslets = typeof(IRTLangParser).Assembly.GetTypes()
                 .Where(x => typeof(IParslet).IsAssignableFrom(x) && x.CustomAttributes.Any())
                 .SelectMany(x =>
                 {
                     return x.GetCustomAttributes(false).Select(y => new
                     {
                         Attribute = (ParsletAttribute)y,
                         Type = x
                     }).ToList();
                 })
                 .ToDictionary(x => (x.Attribute.TokenType, x.Attribute.CanBeginWith), x => Activator.CreateInstance(x.Type) as IParslet);

        private readonly Lexer.Lexer _lexer;

        public bool HasNext { get; private set; }
        public Token Current { get; private set; }

        public Parser(Lexer.Lexer lexer)
        {
            _lexer = lexer;

            HasNext = Peek().Type != TokenType.EndOfCode;
        }

        #region API

        public Expression Next()
        {
            Current = _lexer.Lex();
            var result = GetParslet(Current, true).Accept(this);

            Ensure(TokenType.Semicolon);
            HasNext = Peek().Type != TokenType.EndOfCode;

            return result;
        }

        public Token Match(TokenType tokenType)
        {
            if (Current.Type == tokenType)
            {
                return Take();
            }

            throw new ParserException(Current, $"Expected {tokenType} but found {Current.Type}.");
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
            if (Current.Type != type)
            {
                throw new ParserException(Current, $"Expected {type} but found {Current.Type}.");
            }
        }

        public bool IsEndOfStatement()
            => Current.Type == TokenType.Semicolon || Current.Type == TokenType.EndOfCode;

        private IParslet GetParslet(Token token, bool canBeginWith)
        {
            if (Parslets.TryGetValue((token.Type, canBeginWith), out var value))
            {
                return value;
            }
            throw new ParserException(token, $"Invalid token found: {token.Text}");
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
