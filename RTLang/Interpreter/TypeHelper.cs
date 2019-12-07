using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RTLang
{
    public static partial class TypeHelper
    {
        private static readonly Dictionary<Type, HashSet<Type>> _conversionTable = new Dictionary<Type, HashSet<Type>>
        {
            [typeof(int)] = new HashSet<Type>(new[] { typeof(float), typeof(double), typeof(decimal) }),
            [typeof(float)] = new HashSet<Type>(new[] { typeof(double), typeof(decimal) }),
            [typeof(double)] = new HashSet<Type>(new[] { typeof(decimal) })
        };

        public static bool CanChangeType(Type from, Type to)
        {
            if (from == to)
            {
                return true;
            }

            if (to == typeof(string))
            {
                return true;
            }

            if (to.IsPrimitive && from.IsPrimitive)
            {
                if (_conversionTable.TryGetValue(from, out var toTypes))
                {
                    if (toTypes.Contains(to))
                    {
                        return true;
                    }
                }

                return false;
            }

            return to.IsAssignableFrom(from);
        }

        public static bool TryChangeType(ref object value, Type desiredType)
        {
            if (value != null)
            {
                var actualType = value.GetType();

                if (actualType == desiredType)
                {
                    return true;
                }

                if (desiredType == typeof(string))
                {
                    value = value.ToString();
                    return true;
                }

                if (desiredType == typeof(char))
                {
                    if (value is string str && char.TryParse(str, out char result))
                    {
                        value = result;
                        return true;
                    }

                    return false;
                }

                if (desiredType.IsPrimitive && actualType.IsPrimitive)
                {
                    if (_conversionTable.TryGetValue(actualType, out var toTypes))
                    {
                        if (toTypes.Contains(desiredType))
                        {
                            value = TypeDescriptor.GetConverter(actualType).ConvertTo(value, desiredType);
                            return true;
                        }
                    }

                    return false;
                }

                if (desiredType.IsAssignableFrom(actualType))
                {
                    value = Convert.ChangeType(value, desiredType);
                    return true;
                }

                return false;
            }

            if (desiredType.IsClass)
            {
                return true;
            }

            return false;
        }
    }
}
