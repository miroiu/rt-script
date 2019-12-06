using RTLang.Lexer;

namespace RTLang.CodeAnalysis
{
    public static class Extensions
    {
        public static T[] ToOneItemArray<T>(this T value)
            => new[] { value };

        public static string ToFriendlyString(this TokenType type)
        {
            switch (type)
            {
                case TokenType.Comma:
                    return ",";

                case TokenType.Semicolon:
                    return ";";

                case TokenType.OpenParen:
                    return "(";

                case TokenType.CloseParen:
                    return ")";

                case TokenType.OpenBrace:
                    return "[";

                case TokenType.CloseBrace:
                    return "]";

                case TokenType.Var:
                    return "var";

                case TokenType.Const:
                    return "const";

                case TokenType.Print:
                    return "print";

                case TokenType.True:
                    return "true";

                case TokenType.False:
                    return "false";

                case TokenType.Null:
                    return "null";

                case TokenType.Plus:
                    return "+";

                case TokenType.Minus:
                    return "-";

                case TokenType.Asterisk:
                    return "*";

                case TokenType.Slash:
                    return "/";

                case TokenType.Equals:
                    return "=";

                case TokenType.Exclamation:
                    return "!";

                case TokenType.Dot:
                    return ".";

                case TokenType.LessThan:
                    return "<";

                case TokenType.GreaterThan:
                    return ">";

                case TokenType.ExclamationEquals:
                    return "!=";

                case TokenType.EqualsEquals:
                    return "==";

                case TokenType.EndOfCode:
                    return "\0";

                case TokenType.Identifier:
                    return "identifier";

                case TokenType.Number:
                    return "number";

                case TokenType.String:
                    return "string";

                default:
                    return string.Empty;
            }
        }
    }
}
