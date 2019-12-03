using System;

namespace RTScript.Language.Completion
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class CompletionProviderAttribute : Attribute
    {
        public CompletionProviderAttribute(Type expressionType)
        {
            ExpressionType = expressionType;
        }

        public Type ExpressionType { get; }
    }
}
