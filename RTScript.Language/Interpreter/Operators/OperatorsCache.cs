using RTScript.Language.Expressions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RTScript.Language.Interpreter.Operators
{
    public static class OperatorsCache
    {
        private static readonly Dictionary<UnaryOperatorType, List<IUnaryOperator>> _unaryOperators = new Dictionary<UnaryOperatorType, List<IUnaryOperator>>();
        private static readonly Dictionary<BinaryOperatorType, List<IBinaryOperator>> _binaryOperators = new Dictionary<BinaryOperatorType, List<IBinaryOperator>>();
        private static readonly HashSet<Type> _loadedTypes = new HashSet<Type>();

        public static IUnaryOperator GetUnaryOperator(UnaryOperatorType operatorType, Type type)
        {
            if (_unaryOperators.TryGetValue(operatorType, out var unaryOperators))
            {
                foreach (var op in unaryOperators)
                {
                    if (op.ParameterType.IsAssignableFrom(type))
                    {
                        return op;
                    }
                }
            }

            throw new Exception($"Unary operator {operatorType} is not defined for type {type.ToFriendlyName()}");
        }

        public static IBinaryOperator GetBinaryOperator(BinaryOperatorType operatorType, Type leftType, Type rightType)
        {
            if (_binaryOperators.TryGetValue(operatorType, out var binaryOperators))
            {
                foreach (var op in binaryOperators)
                {
                    if (op.LeftType.IsAssignableFrom(leftType) && op.RightType.IsAssignableFrom(rightType))
                    {
                        return op;
                    }
                }
            }

            throw new Exception($"Binary operator {operatorType} is not defined for types {leftType.ToFriendlyName()} and {rightType.ToFriendlyName()}");
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
                        var unaryAttribute = (UnaryOperatorAttribute)method.GetCustomAttribute(typeof(UnaryOperatorAttribute), false);

                        if (unaryAttribute != default)
                        {
                            var parameters = method.GetParameters();
                            var returnType = method.ReturnType;

                            if (parameters.Length == 1 && returnType != typeof(void))
                            {
                                var parameterType = parameters[0].ParameterType;

                                var unaryType = typeof(UnaryOperator<,>).MakeGenericType(parameterType, returnType);
                                var operatorType = unaryAttribute.OperatorType;

                                var funcType = typeof(Func<,>).MakeGenericType(parameterType, returnType);
                                var funcInst = Delegate.CreateDelegate(funcType, method);

                                var instance = (IUnaryOperator)Activator.CreateInstance(unaryType, funcInst, operatorType);
                                AddOperator(instance);
                            }

                            continue;
                        }

                        var binaryAttribute = (BinaryOperatorAttribute)method.GetCustomAttribute(typeof(BinaryOperatorAttribute), false);

                        if (binaryAttribute != default)
                        {
                            var parameters = method.GetParameters();
                            var returnType = method.ReturnType;

                            if (parameters.Length == 2 && returnType != typeof(void))
                            {
                                var leftType = parameters[0].ParameterType;
                                var rightType = parameters[1].ParameterType;

                                var binaryType = typeof(BinaryOperator<,,>).MakeGenericType(leftType, rightType, returnType);
                                var operatorType = binaryAttribute.OperatorType;

                                var funcType = typeof(Func<,,>).MakeGenericType(leftType, rightType, returnType);
                                var funcInst = Delegate.CreateDelegate(funcType, method);

                                var instance = (IBinaryOperator)Activator.CreateInstance(binaryType, funcInst, operatorType);
                                AddOperator(instance);
                            }

                            continue;
                        }
                    }
                }
            }
        }

        public static void AddOperator(IUnaryOperator op)
        {
            if (!_unaryOperators.TryGetValue(op.OperatorType, out var operators))
            {
                _unaryOperators.Add(op.OperatorType, new List<IUnaryOperator>() { op });
            }
            else
            {
                operators.Add(op);
            }
        }

        public static void AddOperator(IBinaryOperator op)
        {
            if (!_binaryOperators.TryGetValue(op.OperatorType, out var operators))
            {
                _binaryOperators.Add(op.OperatorType, new List<IBinaryOperator>() { op });
            }
            else
            {
                operators.Add(op);
            }
        }
    }
}
