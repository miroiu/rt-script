using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    public static class ParserHelpers
    {
        public static LiteralType ToLiteralType(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.True:
                    return LiteralType.Boolean;

                case TokenType.False:
                    return LiteralType.Boolean;

                case TokenType.Null:
                    return LiteralType.Null;

                case TokenType.Number:
                    return LiteralType.Number;

                case TokenType.String:
                    return LiteralType.Null;

            }

            throw new ParserException(token, $"{token.Type} is not a literal type.");
        }

        public static bool IsBinaryOperator(this TokenType type)
        {
            switch (type)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                case TokenType.Asterisk:
                case TokenType.Slash:
                case TokenType.GreaterThan:
                case TokenType.LessThan:
                case TokenType.EqualsEquals:
                case TokenType.ExclamationEquals:
                    return true;
            }

            return false;
        }

        public static bool IsUnaryOperator(this TokenType type)
        {
            switch (type)
            {
                case TokenType.Exclamation:
                    return true;

                case TokenType.Minus:
                    return true;
            }

            return false;
        }

        public static UnaryOperatorType ToUnaryOperatorType(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.Print:
                    return UnaryOperatorType.Print;

                case TokenType.Exclamation:
                    return UnaryOperatorType.LogicalNegation;

                case TokenType.Minus:
                    return UnaryOperatorType.Minus;
            }

            throw new ParserException(token, $"{token.Type} is not an unary operator.");
        }

        public static BinaryOperatorType ToBinaryOperatorType(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.Plus:
                    return BinaryOperatorType.Plus;

                case TokenType.Minus:
                    return BinaryOperatorType.Minus;

                case TokenType.Asterisk:
                    return BinaryOperatorType.Multiply;

                case TokenType.Slash:
                    return BinaryOperatorType.Divide;

                case TokenType.GreaterThan:
                    return BinaryOperatorType.Greater;

                case TokenType.LessThan:
                    return BinaryOperatorType.Less;

                case TokenType.EqualsEquals:
                    return BinaryOperatorType.Equal;

                case TokenType.ExclamationEquals:
                    return BinaryOperatorType.NotEqual;
            }

            throw new ParserException(token, $"{token.Type} is not a binary operator.");
        }

        // Does not handle prefix operators because that's handled in the binary parslet code
        public static OperatorPrecedence ToOperatorPrecedence(this Token token)
        {
            switch (token.Type)
            {
                case TokenType.GreaterThan:
                case TokenType.LessThan:
                case TokenType.EqualsEquals:
                case TokenType.ExclamationEquals:
                    return OperatorPrecedence.Equality;

                case TokenType.Plus:
                case TokenType.Minus:
                    return OperatorPrecedence.Addition;

                case TokenType.Asterisk:
                case TokenType.Slash:
                    return OperatorPrecedence.Multiplication;
            }

            throw new ParserException(token, $"{token.Type} is not an operator.");
        }
    }
}
