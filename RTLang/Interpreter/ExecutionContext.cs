using RTScript;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RTScript.Interpreter
{
    public class ExecutionContext : IExecutionContext
    {
        private readonly IDictionary<string, Reference> _variables = new Dictionary<string, Reference>();
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
            if (_variables.ContainsKey(name))
            {
                throw new Exception($"'{name}' is already defined in this scope.");
            }

            _variables[name] = new Reference(name, isConst, value);
        }

        public object GetValue(string name)
        {
            if (!_variables.TryGetValue(name, out var result))
            {
                throw new Exception($"'{name}' does not exist in the current context.");
            }

            return result.Value;
        }

        public Type GetType(string name)
        {
            if (!_variables.TryGetValue(name, out var result))
            {
                throw new Exception($"'{name}' does not exist in the current context.");
            }

            return result.Type;
        }

        public void Print(object value)
        {
            if (value is ICollection collection)
            {
                StringBuilder builder = new StringBuilder(12);
                builder.Append("[");

                foreach (var element in collection)
                {
                    var friendly = ToFriendlyString(element);
                    builder.Append($"{friendly}, ");
                }

                if (collection.Count > 0)
                {
                    builder.Length -= 2;
                }
                builder.Append("]");
                _out.WriteLine(builder.ToString());
            }
            else
            {
                _out.WriteLine(ToFriendlyString(value));
            }
        }

        private string ToFriendlyString(object value)
        {
            var result = value?.ToString() ?? "null";
            return result == "True" ? "true" : result == "False" ? "false" : result;
        }

        public object Evaluate(LiteralType type, string value)
        {
            // TODO: Could be improved by caching common values and already parsed values
            switch (type)
            {
                case LiteralType.Boolean:
                    return bool.Parse(value);

                // TODO: Let user specify default number type?
                case LiteralType.Number:
                    if (int.TryParse(value, out int result))
                    {
                        return result;
                    }

                    if (double.TryParse(value, out double dResult))
                    {
                        return dResult;
                    }

                    throw new Exception($"{value} is not a number. (should not happen)");

                case LiteralType.String:
                    return value;

                case LiteralType.Null:
                default:
                    return null;
            }
        }

        public object Evaluate(UnaryOperatorType operatorType, object value)
        {
            if (value != null)
            {
                var type = value.GetType();
                var op = OperatorsCache.GetUnaryOperator(operatorType, type);
                if (op != null)
                {
                    return op.Execute(value);
                }

                throw new Exception($"Operator '{operatorType.ToFriendlyName()}' is not defined for type '{type.ToFriendlyName()}'");
            }

            throw new Exception($"Operator '{operatorType.ToFriendlyName()}' cannot be applied to null values.");
        }

        public object Evaluate(BinaryOperatorType operatorType, object left, object right)
        {
            if (left != null && right != null)
            {
                // Can throw null reference exception
                var leftType = left.GetType();
                var rightType = right.GetType();

                if (operatorType == BinaryOperatorType.Plus && (leftType == typeof(string) || rightType == typeof(string)))
                {
                    return $"{left}{right}";
                }

                var op = OperatorsCache.GetBinaryOperator(operatorType, leftType, rightType);
                if (op != null && TypeHelper.TryChangeType(ref left, op.LeftType) && TypeHelper.TryChangeType(ref right, op.RightType))
                {
                    return op.Execute(left, right);
                }

                throw new Exception($"Operator '{operatorType.ToFriendlyName()}' is not defined for types '{leftType.ToFriendlyName()}' and '{rightType.ToFriendlyName()}'.");
            }

            throw new Exception($"Operator '{operatorType.ToFriendlyName()}' cannot be applied to null values.");
        }
    }
}
