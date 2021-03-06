﻿namespace RTLang.Lexer
{
    public partial class Lexer
    {
        private readonly SourceText _text;

        public Lexer(SourceText text)
            => _text = text;

        public Token Lex()
        {
            Token token = new Token
            {
                Text = $"{_text.Current}",
                Line = _text.Line,
                Column = _text.Column,
                Position = _text.Position
            };

            switch (_text.Current)
            {
                case '\0':
                    token.Type = TokenType.EndOfCode;
                    break;

                case '+':
                    token.Type = TokenType.Plus;
                    _text.MoveNext();
                    break;

                case '-':
                    token.Type = TokenType.Minus;
                    _text.MoveNext();
                    break;

                case '/':
                    token.Type = TokenType.Slash;
                    _text.MoveNext();

                    if (_text.Current == '/')
                    {
                        ReadComment(_text);
                        return Lex();
                    }
                    break;

                case '*':
                    token.Type = TokenType.Asterisk;
                    _text.MoveNext();
                    break;

                case '<':
                    token.Type = TokenType.LessThan;
                    _text.MoveNext();
                    break;

                case '>':
                    token.Type = TokenType.GreaterThan;
                    _text.MoveNext();
                    break;

                case '(':
                    token.Type = TokenType.OpenParen;
                    _text.MoveNext();
                    break;

                case ')':
                    token.Type = TokenType.CloseParen;
                    _text.MoveNext();
                    break;

                case '[':
                    token.Type = TokenType.OpenBrace;
                    _text.MoveNext();
                    break;

                case ']':
                    token.Type = TokenType.CloseBrace;
                    _text.MoveNext();
                    break;

                case ';':
                    token.Type = TokenType.Semicolon;
                    _text.MoveNext();
                    break;

                case ',':
                    token.Type = TokenType.Comma;
                    _text.MoveNext();
                    break;

                case '.':
                    token.Type = TokenType.Dot;
                    _text.MoveNext();
                    break;

                case '!':
                    token.Type = TokenType.Exclamation;
                    _text.MoveNext();

                    if (_text.Current == '=')
                    {
                        token.Type = TokenType.ExclamationEquals;
                        _text.MoveNext();
                    }
                    break;

                case '=':
                    token.Type = TokenType.Equals;
                    _text.MoveNext();

                    if (_text.Current == '=')
                    {
                        token.Type = TokenType.EqualsEquals;
                        _text.MoveNext();
                    }
                    break;

                case ' ':
                case '\t':
                    ReadWhiteSpace(_text);
                    return Lex();

                case '\r':
                case '\n':
                    _text.MoveNext();
                    return Lex();

                case '"':
                case '\'':
                    token.Type = TokenType.String;
                    token.Text = ReadString(_text);
                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    token.Type = TokenType.Number;
                    token.Text = ReadNumber(_text);
                    break;

                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                case '_':
                    (token.Type, token.Text) = ReadIdentifierOrKeyword(_text);
                    break;

                default:
                    throw new LexerException(_text.Line, _text.Column, $"Unexpected character {_text.Current}");
            }

            return token;
        }

        public Token Peek(int offset = 1)
        {
            var returnTo = _text.Position;
            var col = _text.Column;
            var row = _text.Line;

            Token token = default;

            for (int i = 0; i < offset; i++)
            {
                token = Lex();
            }

            _text.Position = returnTo;
            _text.Column = col;
            _text.Line = row;

            return token;
        }
    }
}
