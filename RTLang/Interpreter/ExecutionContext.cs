﻿using System;
using System.Collections.Generic;

namespace RTLang.Interpreter
{
    public sealed class ExecutionContext : IExecutionContext
    {
        private readonly IDictionary<string, Reference> _variables = new Dictionary<string, Reference>();
        private readonly IDictionary<string, Type> _types = new Dictionary<string, Type>();
        private readonly IOutputStream _out;

        public ExecutionContext(IOutputStream outs)
            => _out = outs;

        public void Assign(string name, object value)
        {
            if (!_variables.TryGetValue(name, out var reference))
            {
                throw new Exception($"'{name}' does not exist in the current context.");
            }

            reference.SetValue(value);
        }

        public void Declare(string name, object value, bool isConst = false)
        {
            if (_variables.ContainsKey(name) || _types.ContainsKey(name))
            {
                throw new Exception($"'{name}' is already defined in the current context.");
            }

            _variables[name] = new Reference(name, isConst, value);
        }

        public void Declare(Type type)
        {
            var name = type.ToFriendlyName();

            if (_variables.ContainsKey(name) || _types.ContainsKey(name))
            {
                throw new Exception($"'{name}' is already defined in the current context.");
            }

            _types[name] = type;
        }

        public object GetValue(string name)
        {
            if (_types.ContainsKey(name))
            {
                return default;
            }

            if (!_variables.TryGetValue(name, out var result))
            {
                throw new Exception($"'{name}' does not exist in the current context.");
            }

            return result.Value;
        }

        public Type GetType(string name)
        {
            if (_types.TryGetValue(name, out var type))
            {
                return type;
            }

            if (!_variables.TryGetValue(name, out var result))
            {
                throw new Exception($"'{name}' does not exist in the current context.");
            }

            return result.Type;
        }

        public bool IsType(string name)
            => _types.ContainsKey(name);

        public bool IsReadOnly(string name)
        {
            if (_types.ContainsKey(name))
            {
                return true;
            }

            if (!_variables.TryGetValue(name, out var result))
            {
                throw new Exception($"'{name}' does not exist in the current context.");
            }

            return result.IsReadOnly;
        }

        public void Print(object value)
            => _out.WriteLine(value.ToFriendlyString());

        public object Evaluate(LiteralType type, string value)
        {
            switch (type)
            {
                case LiteralType.Boolean:
                    return bool.Parse(value);

                case LiteralType.Number:
                    if (int.TryParse(value, out int result))
                    {
                        return result;
                    }

                    if (float.TryParse(value, out float dResult))
                    {
                        return dResult;
                    }

                    throw new Exception($"'{value}' is not a number. (should not happen)");

                case LiteralType.String:
                    return value.Substring(1, value.Length - 2);

                case LiteralType.Null:
                default:
                    return default;
            }
        }

        public object Evaluate(UnaryOperatorType operatorType, object value)
        {
            if (value != default)
            {
                var type = value.GetType();

                var op = TypeHelper.GetUnaryOperator(operatorType, type);
                if (op != default && TypeHelper.TryChangeType(ref value, op.ParameterType))
                {
                    return op.Execute(value);
                }

                throw new Exception($"Operator '{operatorType.ToFriendlyName()}' is not defined for type '{type.ToFriendlyName()}'");
            }

            throw new Exception($"Operator '{operatorType.ToFriendlyName()}' cannot be applied to null values.");
        }

        public object Evaluate(BinaryOperatorType operatorType, object left, object right)
        {
            if (left != default && right != default)
            {
                var leftType = left.GetType();
                var rightType = right.GetType();

                if (operatorType == BinaryOperatorType.Plus && (leftType == typeof(string) || rightType == typeof(string)))
                {
                    return $"{left}{right}";
                }

                var op = TypeHelper.GetBinaryOperator(operatorType, leftType, rightType);
                if (op != default && TypeHelper.TryChangeType(ref left, op.LeftType) && TypeHelper.TryChangeType(ref right, op.RightType))
                {
                    return op.Execute(left, right);
                }

                throw new Exception($"Operator '{operatorType.ToFriendlyName()}' is not defined for types '{leftType.ToFriendlyName()}' and '{rightType.ToFriendlyName()}'.");
            }

            throw new Exception($"Operator '{operatorType.ToFriendlyName()}' cannot be applied to null values.");
        }
    }
}
