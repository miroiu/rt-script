using System.Text;

namespace RTLang.Lexer
{
    public partial class Lexer
    {
        private (TokenType Type, string Text) ReadIdentifierOrKeyword(SourceText stream)
        {
            StringBuilder builder = new StringBuilder(12);

            while (char.IsLetterOrDigit(stream.Current) || stream.Current == '_')
            {
                builder.Append(stream.Current);
                stream.MoveNext();
            }

            var text = builder.ToString();

            return (ToKeyword(text), text);
        }

        private string ReadNumber(SourceText stream)
        {
            StringBuilder builder = new StringBuilder(8);
            bool hasDot = false;

            while (true)
            {
                if (stream.Current == '.')
                {
                    if (!hasDot)
                    {
                        hasDot = true;

                        builder.Append(stream.Current);
                        stream.MoveNext();
                    }
                    else
                    {
                        throw new LexerException(_text.Line, _text.Column, $"Invalid number format.");
                    }
                }
                else if (char.IsDigit(stream.Current))
                {
                    builder.Append(stream.Current);
                    stream.MoveNext();
                }
                else
                {
                    break;
                }
            }

            if (stream.Peek(-1) == '.')
            {
                throw new LexerException(_text.Line, _text.Column, $"Invalid number format.");
            }

            return builder.ToString();
        }

        private string ReadString(SourceText stream)
        {
            char delimiter = stream.Current;

            StringBuilder builder = new StringBuilder(10);
            builder.Append(stream.Current);
            bool finished = false;

            while (!finished)
            {
                if (!stream.MoveNext())
                {
                    throw new LexerException(stream.Line, stream.Column, "Unterminated string.");
                }
                else if (stream.Current == delimiter)
                {
                    finished = true;
                    builder.Append(stream.Current);
                    stream.MoveNext();
                }
                else
                {
                    builder.Append(stream.Current);
                }
            }

            return builder.ToString();
        }

        private void ReadWhiteSpace(SourceText stream)
        {
            while (char.IsWhiteSpace(stream.Current) && stream.MoveNext()) ;
        }

        private void ReadComment(SourceText stream)
        {
            while (stream.Current != '\n' && stream.MoveNext()) ;
        }

        private TokenType ToKeyword(string value)
        {
            switch (value)
            {
                case "var":
                    return TokenType.Var;

                case "const":
                    return TokenType.Const;

                case "print":
                    return TokenType.Print;

                case "null":
                    return TokenType.Null;

                case "true":
                    return TokenType.True;

                case "false":
                    return TokenType.False;
            }

            return TokenType.Identifier;
        }
    }
}
