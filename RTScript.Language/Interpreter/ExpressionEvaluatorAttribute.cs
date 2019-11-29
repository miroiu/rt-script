using System;

namespace RTScript.Language.Interpreter
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ExpressionEvaluatorAttribute : Attribute
    {
        public Type ExpressionType { get; private set; }

        public ExpressionEvaluatorAttribute(Type expressionType)
        {
            ExpressionType = expressionType;
        }
    }
}
