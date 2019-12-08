using System;
using System.Collections.Generic;
using System.Linq;
using RTLang.Interop;
using RTLang.Interpreter;
using RTLang.Parser;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(InvocationExpression))]
    internal class InvocationAnalyzer : IExpressionAnalyzer
    {
        private const string DelegateInvoke = "Invoke";

        public IEnumerable<Completion> GetCompletions(Expression expression, IAnalysisContext context)
        {
            var casted = (InvocationExpression)expression;
            var delegateType = context.GetSymbolType(casted.MethodName);

            return GetMethodOverloadsCompletion(delegateType, casted.MethodName);
        }

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (InvocationExpression)expression;
            var delegateType = context.GetSymbolType(casted.MethodName);

            if (delegateType == default)
            {
                return new Diagnostic
                {
                    Position = casted.Token.Position,
                    Length = casted.Token.Text.Length,
                    Type = DiagnosticType.Error,
                    Message = $"'{casted.MethodName}' is not defined in the current context."
                }.ToOneItemArray();
            }

            bool exists = TypeHelper.GetMethods(delegateType).Any(m => m.Descriptor.Name == DelegateInvoke && IsMethodOverload(m.Descriptor, casted.Arguments.Items));
            if (!exists)
            {
                return new Diagnostic
                {
                    Position = casted.Token.Position,
                    Length = casted.Token.Text.Length,
                    Type = DiagnosticType.Error,
                    Message = $"No matching overload found for '{casted.MethodName}'"
                }.ToOneItemArray();
            }

            return AnalyzerService.GetDiagnostics(casted.Arguments, context);
        }

        public Type GetReturnType(Expression expression, IAnalysisContext context)
        {
            var casted = (InvocationExpression)expression;
            var delegateType = context.GetSymbolType(casted.MethodName);

            if (delegateType != default)
            {
                return TypeHelper.GetMethods(delegateType)
                    .FirstOrDefault(m => m.Descriptor.Name == DelegateInvoke && IsMethodOverload(m.Descriptor, casted.Arguments.Items))
                    ?.Descriptor.ReturnType;
            }

            return default;
        }

        public static bool IsMethodOverload(MethodDescriptor descriptor, IReadOnlyList<Expression> parameters)
        {
            // TODO:
            return descriptor.Parameters.Count == parameters.Count;
        }

        public static IEnumerable<Completion> GetMethodOverloadsCompletion(Type type, string methodName)
        {
            bool isDelegate = typeof(Delegate).IsAssignableFrom(type);
            var name = isDelegate ? DelegateInvoke : methodName;

            return TypeHelper.GetMethods(type)
                .Where(m => m.Descriptor.Name == name)
                .Select(m => new Completion
                {
                    Text = BuildMethodSignature(m.Descriptor, methodName),
                    Type = SymbolType.MethodSignature
                });
        }

        private static string BuildMethodSignature(MethodDescriptor descriptor, string methodName)
        {
            var args = descriptor.Parameters.Select(p => p.ToFriendlyName());
            return $"{descriptor.ReturnType.ToFriendlyName()} {methodName}({string.Join(", ", args)})";
        }
    }
}
