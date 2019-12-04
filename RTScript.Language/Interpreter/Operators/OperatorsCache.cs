﻿using RTScript.Language.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace RTScript.Language.Interpreter.Operators
{
    public static class OperatorsCache
    {
        static OperatorsCache()
        {
            LoadOperators(typeof(NumberOperators));
            LoadOperators(typeof(BooleanOperators));
        }

        private static readonly Dictionary<UnaryOperatorType, Dictionary<Type, IUnaryOperator>> _unaryOperators = new Dictionary<UnaryOperatorType, Dictionary<Type, IUnaryOperator>>();
        private static readonly Dictionary<BinaryOperatorType, Dictionary<(Type Left, Type Right), IBinaryOperator>> _binaryOperators = new Dictionary<BinaryOperatorType, Dictionary<(Type, Type), IBinaryOperator>>();
        private static readonly HashSet<Type> _loadedTypes = new HashSet<Type>();

        public static IUnaryOperator GetUnaryOperator(UnaryOperatorType operatorType, Type type)
        {
            if (_unaryOperators.TryGetValue(operatorType, out var unaryOperators))
            {
                if (unaryOperators.TryGetValue(type, out var op))
                {
                    return op;
                }
            }

            throw new Exception($"Unary operator {operatorType} is not defined for type {type.ToFriendlyName()}");
        }

        public static IBinaryOperator GetBinaryOperator(BinaryOperatorType operatorType, Type leftType, Type rightType)
        {
            if (_binaryOperators.TryGetValue(operatorType, out var binaryOperators))
            {
                if (binaryOperators.TryGetValue((leftType, rightType), out var op))
                {
                    return op;
                }
                else
                {
                    foreach (var bOp in binaryOperators)
                    {
                        if (CanConvertType(leftType, bOp.Key.Left) && CanConvertType(rightType, bOp.Key.Right))
                        {
                            return bOp.Value;
                        }
                    }
                }
            }

            throw new Exception($"Binary operator {operatorType} is not defined for types {leftType.ToFriendlyName()} and {rightType.ToFriendlyName()}");
        }

        private static bool CanConvertType(Type from, Type to)
        {
            if (from.IsPrimitive)
            {
                return TypeDescriptor.GetConverter(from).CanConvertTo(to);
            }

            return to.IsAssignableFrom(from);
        }

        public static void AddOperator(IUnaryOperator op)
        {
            if (!_unaryOperators.TryGetValue(op.OperatorType, out var operators))
            {
                _unaryOperators.Add(op.OperatorType, new Dictionary<Type, IUnaryOperator> { [op.ParameterType] = op });
            }
            else if (!operators.TryGetValue(op.ParameterType, out _))
            {
                operators.Add(op.ParameterType, op);
            }
        }

        public static void AddOperator(IBinaryOperator op)
        {
            if (!_binaryOperators.TryGetValue(op.OperatorType, out var operators))
            {
                _binaryOperators.Add(op.OperatorType, new Dictionary<(Type, Type), IBinaryOperator> { [(op.LeftType, op.RightType)] = op });
            }
            else if (!operators.TryGetValue((op.LeftType, op.RightType), out var _))
            {
                operators.Add((op.LeftType, op.RightType), op);
            }
        }

        public static void LoadOperators<T>() where T : class
            => LoadOperators(typeof(T));

        public static void LoadOperators(Type host)
        {
            if (host.IsClass)
            {
                if (_loadedTypes.Add(host))
                {
                    var staticMethods = host.GetMethods(BindingFlags.Public | BindingFlags.Static);

                    foreach (var method in staticMethods)
                    {
                        var attribute = (OperatorAttribute)method.GetCustomAttribute(typeof(OperatorAttribute), false);

                        if (attribute?.IsUnary ?? false)
                        {
                            if (TryGetUnaryOperator(method, attribute.UnaryOperatorType, out var unary))
                            {
                                AddOperator(unary);
                            }
                        }
                        else
                        {
                            if (TryGetBinaryOperator(method, attribute.BinaryOperatorType, out var binary))
                            {
                                AddOperator(binary);
                            }
                        }
                    }
                }
            }
        }

        private static bool TryGetUnaryOperator(MethodInfo method, UnaryOperatorType operatorType, out IUnaryOperator result)
        {
            result = default;
            var parameters = method.GetParameters();
            var returnType = method.ReturnType;

            if (parameters.Length == 1 && returnType != typeof(void))
            {
                var parameterType = parameters[0].ParameterType;

                var unaryType = typeof(UnaryOperator<,>).MakeGenericType(parameterType, returnType);

                var funcType = typeof(Func<,>).MakeGenericType(parameterType, returnType);
                var funcInst = Delegate.CreateDelegate(funcType, method);

                result = (IUnaryOperator)Activator.CreateInstance(unaryType, funcInst, operatorType);
                return true;
            }

            return false;
        }

        private static bool TryGetBinaryOperator(MethodInfo method, BinaryOperatorType operatorType, out IBinaryOperator result)
        {
            result = default;
            var parameters = method.GetParameters();
            var returnType = method.ReturnType;

            if (parameters.Length == 2 && returnType != typeof(void))
            {
                var leftType = parameters[0].ParameterType;
                var rightType = parameters[1].ParameterType;

                var binaryType = typeof(BinaryOperator<,,>).MakeGenericType(leftType, rightType, returnType);

                var funcType = typeof(Func<,,>).MakeGenericType(leftType, rightType, returnType);
                var funcInst = Delegate.CreateDelegate(funcType, method);

                result = (IBinaryOperator)Activator.CreateInstance(binaryType, funcInst, operatorType);
                return true;
            }

            return false;
        }
    }
}
