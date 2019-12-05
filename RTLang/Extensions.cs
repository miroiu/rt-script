using System;

namespace RTLang
{
    public static class Extensions
    {
        #region Strings

        public static string ToFriendlyName(this Type type)
        {
            if (type.Name == typeof(float).Name)
            {
                return "float";
            }

            if (type.Name == typeof(int).Name)
            {
                return "int";
            }

            if (type.Name == typeof(bool).Name)
            {
                return "bool";
            }

            if (type.Name == typeof(string).Name)
            {
                return "string";
            }

            if (type.Name == typeof(double).Name)
            {
                return "double";
            }

            if (type.Name == typeof(decimal).Name)
            {
                return "decimal";
            }

            if (type.Name == typeof(char).Name)
            {
                return "char";
            }

            if (type.Name == typeof(object).Name)
            {
                return "object";
            }

            if (type.Name == typeof(void).Name)
            {
                return "void";
            }

            if (type.IsArray)
            {
                return $"{type.GetElementType().ToFriendlyName()}[]";
            }

            return type.FullName;
        }

        public static string ToFriendlyName(this BinaryOperatorType type)
        {
            switch (type)
            {
                case BinaryOperatorType.Plus:
                    return "+";

                case BinaryOperatorType.Minus:
                    return "-";

                case BinaryOperatorType.Multiply:
                    return "*";

                case BinaryOperatorType.Divide:
                    return "/";

                case BinaryOperatorType.Greater:
                    return ">";

                case BinaryOperatorType.Less:
                    return "<";

                case BinaryOperatorType.Equal:
                    return "==";

                case BinaryOperatorType.NotEqual:
                    return "!=";

                case BinaryOperatorType.Assign:
                    return "=";

                case BinaryOperatorType.AccessMember:
                    return ".";
            }

            throw new Exception($"{nameof(ToFriendlyName)}: should not happen.");
        }

        public static string ToFriendlyName(this UnaryOperatorType type)
        {
            switch (type)
            {
                case UnaryOperatorType.Minus:
                    return "-";

                case UnaryOperatorType.LogicalNegation:
                    return "!";

                case UnaryOperatorType.Print:
                    return "print";
            }

            throw new Exception($"{nameof(ToFriendlyName)}: should not happen.");
        }

        #endregion

        public static void DeclareStatic<T>(this IExecutionContext context, string name)
            => context.Declare(name, typeof(T));

        public static void DeclareStatic<T>(this IExecutionContext context)
            => context.Declare(typeof(T).ToFriendlyName(), typeof(T));
    }
}
