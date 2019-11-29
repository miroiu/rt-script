using System;

namespace RTScript.Language
{
    public static class Extensions
    {
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

            if (type.Name == typeof(decimal).Name)
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

            return type.Name;
        }
    }
}
