using RTLang.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

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

        private static readonly Dictionary<(Type From, Type To), MethodInfo> _conversionOperators = new Dictionary<(Type From, Type To), MethodInfo>();

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
                    return TryConvertStringToChar(ref value);
                }

                if (desiredType.IsPrimitive && actualType.IsPrimitive)
                {
                    return TryConvertPrimitive(ref value, actualType, desiredType);
                }

                if (desiredType.IsAssignableFrom(actualType))
                {
                    value = Convert.ChangeType(value, desiredType);
                    return true;
                }
            }

            if (desiredType.IsClass)
            {
                return true;
            }

            return false;
        }

        private static bool TryConvertStringToChar(ref object value)
        {
            if (value is string str && char.TryParse(str, out char result))
            {
                value = result;
                return true;
            }

            return false;
        }

        private static bool TryConvertPrimitive(ref object value, Type actualType, Type desiredType)
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
    }
}
