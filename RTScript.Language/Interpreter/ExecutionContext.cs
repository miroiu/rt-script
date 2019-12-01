using RTScript.Language.Expressions;
using RTScript.Language.Interpreter.Operators;
using System;
using System.Collections.Generic;

namespace RTScript.Language.Interpreter
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

        public object Get(string name)
        {
            if (!_variables.TryGetValue(name, out var result))
            {
                throw new Exception($"'{name}' does not exist in the current context.");
            }

            return result.Value;
        }

        public void Print(object value)
        {
            var result = value?.ToString() ?? "null";
            _out.WriteLine(result == "True" ? "true" : result == "False" ? "false" : result);
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
                    return double.Parse(value);

                case LiteralType.String:
                    return value;

                case LiteralType.Null:
                default:
                    return null;
            }
        }

        public object Evaluate(UnaryOperatorType operatorType, object value)
        {
            // Can throw null reference exception
            var type = value.GetType();
            var op = OperatorsCache.GetUnaryOperator(operatorType, type);
            return op.Execute(value);
        }

        public object Evaluate(object left, BinaryOperatorType operatorType, object right)
        {
            // Can throw null reference exception
            var leftType = left.GetType();
            var rightType = right.GetType();

            var op = OperatorsCache.GetBinaryOperator(operatorType, leftType, rightType);
            return op.Execute(left, right);
        }
    }
}
