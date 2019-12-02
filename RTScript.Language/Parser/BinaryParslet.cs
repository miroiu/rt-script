using RTScript.Language.Expressions;
using RTScript.Language.Lexer;

namespace RTScript.Language.Parser
{
    [Parslet(TokenType.Identifier, true)]
    public class BinaryParslet : IParslet
    {
        public Expression Accept(RTScriptParser parser)
            => ParseBinaryExpression(parser);

        private Expression ParseBinaryExpression(RTScriptParser parser, Expression left = default, OperatorPrecedence parentPrecedence = OperatorPrecedence.None)
        {
            if (left == default)
            {
                // No need for unary parslet
                if (IsUnaryOperator(parser.Current.Type))
                {
                    var operatorToken = parser.Take();
                    var operatorType = GetUnaryOperatorType(operatorToken);
                    left = new UnaryExpression(operatorType, ParseBinaryExpression(parser, left, OperatorPrecedence.Prefix))
                    {
                        Token = operatorToken
                    };
                }
                else
                {
                    left = parser.ParsePrimaryExpression();

                    if (parser.Current.Type == TokenType.Exclamation)
                    {
                        var operatorToken = parser.Take();
                        var operatorType = GetUnaryOperatorType(operatorToken);
                        left = new UnaryExpression(operatorType, left)
                        {
                            Token = operatorToken
                        };
                    }
                }
            }

            while (!parser.IsEndOfStatement())
            {
                if (IsBinaryOperator(parser.Current.Type))
                {
                    var precedence = GetOperatorPrecedence(parser.Current);
                    if (parentPrecedence >= precedence)
                    {
                        return left;
                    }

                    var operatorToken = parser.Take();
                    var operatorType = GetBinaryOperatorType(operatorToken);
                    left = new BinaryExpression(left, operatorType, ParseBinaryExpression(parser, parentPrecedence: precedence))
                    {
                        Token = operatorToken
                    };
                }
                else
                {
                    return left;
                }
            }

            return left;
        }

        public static OperatorPrecedence GetOperatorPrecedence(Token token)
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

                case TokenType.Equals:
                    return OperatorPrecedence.Assignment;
            }

            throw new ParserException(token, $"{token.Type} is not an operator.");
        }

        public static UnaryOperatorType GetUnaryOperatorType(Token token)
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

        public static BinaryOperatorType GetBinaryOperatorType(Token token)
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

                case TokenType.Equals:
                    return BinaryOperatorType.Assign;
            }

            throw new ParserException(token, $"{token.Type} is not a binary operator.");
        }

        public static bool IsUnaryOperator(TokenType type)
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

        public static bool IsBinaryOperator(TokenType type)
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
                case TokenType.Equals:
                    return true;
            }

            return false;
        }
    }
}
