﻿using System;

namespace RTLang.Lexer
{
    public struct Token : IEquatable<Token>
    {
        public TokenType Type;
        public string Text;
        public int Line;
        public int Column;
        public int Position;

        public override bool Equals(object obj)
            => obj is Token token ? token.Equals(this) : false;

        public bool Equals(Token other)
            => other.Type == Type && other.Text == Text;

        public static bool operator ==(Token left, Token right)
            => left.Equals(right);

        public static bool operator !=(Token left, Token right)
            => !(left == right);

        public override int GetHashCode()
            => Text.GetHashCode();
    }
}