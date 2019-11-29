using RTScript.Language.Expressions;
using RTScript.Language.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTScript.Language.Parser
{
    public class RTScriptParser : IExpressionProvider
    {
        public static readonly IDictionary<(TokenType Type, bool CanBeginWith), IParslet> Parslets = typeof(RTScriptParser).Assembly.GetTypes()
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

        internal const TokenType BinaryHackType = TokenType.Asterisk;

        private readonly RTScriptLexer _lexer;

        public bool HasNext { get; private set; }
        public Token Current { get; private set; }

        public RTScriptParser(RTScriptLexer lexer)
        {
            _lexer = lexer;

            HasNext = Peek().Type != TokenType.EndOfCode;
        }

        public Expression Next()
        {
            Current = _lexer.Lex();
            var result = GetParslet(Current, true).Accept(this);

            Ensure(TokenType.Semicolon);
            HasNext = Peek().Type != TokenType.EndOfCode;

            return result;
        }

        public Expression ParseExpression()
            => Parslets[(BinaryHackType, false)].Accept(this);

        public Expression ParsePrimaryExpression()
            => GetParslet(Current, false).Accept(this);

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
            => Current.Type == TokenType.Semicolon;

        private IParslet GetParslet(Token token, bool canBeginWith)
        {
            if (Parslets.TryGetValue((token.Type, canBeginWith), out var value))
            {
                return value;
            }
            throw new ParserException(token, $"Invalid token found: {token.Text}");
        }
    }
}
