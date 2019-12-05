using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace RTLang
{
    public static class Extensions
    {
        #region Name

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

            if (type.IsGenericType)
            {
                return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(x => x.ToFriendlyName()).ToArray()) + ">";
            }

            return type.Name;
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

        #region String

        public static string ToFriendlyString(this object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is bool b)
            {
                return b ? "true" : "false";
            }

            if (value is IDictionary dictionary)
            {
                return dictionary.ToFriendlyString();
            }

            if (value is ICollection collection)
            {
                return collection.ToFriendlyString();
            }

            return value.ToString();
        }

        private static string ToFriendlyString(this IDictionary dictionary)
        {
            StringBuilder builder = new StringBuilder(12);
            builder.Append("{");

            foreach (var key in dictionary.Keys)
            {
                var keyValue = dictionary[key];
                builder.Append($"[{key.ToFriendlyString()}] = {keyValue.ToFriendlyString()}, ");
            }

            if (dictionary.Count > 0)
            {
                builder.Length -= 2;
            }

            builder.Append("}");

            return builder.ToString();
        }

        public static string ToFriendlyString(this ICollection collection)
        {
            StringBuilder builder = new StringBuilder(12);
            builder.Append("[");

            foreach (var element in collection)
            {
                var friendly = element.ToFriendlyString();
                builder.Append($"{friendly}, ");
            }

            if (collection.Count > 0)
            {
                builder.Length -= 2;
            }

            builder.Append("]");
            return builder.ToString();
        }

        #endregion

        public static void DeclareStatic<T>(this IExecutionContext context, string name)
            => context.Declare(name, typeof(T));

        public static void DeclareStatic<T>(this IExecutionContext context)
            => context.Declare(typeof(T).ToFriendlyName(), typeof(T));
    }
}
