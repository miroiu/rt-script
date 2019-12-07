using System;
using System.Collections.Generic;
using System.Linq;
using RTLang.Interop;
using RTLang.Interpreter;
using RTLang.Parser;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(BinaryExpression))]
    internal class BinaryExpressionAnalyzer : IExpressionAnalyzer, ITypeAnalyzer
    {
        public IEnumerable<CompletionItem> GetCompletions(Expression expression, IAnalysisContext context)
        {
            var casted = (BinaryExpression)expression;

            switch (casted.OperatorType)
            {
                case BinaryOperatorType.AccessMember:
                    var type = AnalyzerService.GetReturnType(casted.Left, context);

                    if (type != default)
                    {
                        if (casted.Right is IdentifierExpression property)
                        {
                            return context.GetMembers(type)
                                .Where(s => s.Name != property.Name && s.Name.StartsWith(property.Name))
                                .Select(s => new CompletionItem
                                {
                                    Text = s.Name,
                                    Type = s.Type
                                });
                        }
                    }

                    break;

                case BinaryOperatorType.Assign:
                    break;

                default:
                    return GetCompletions(casted.Right, context);
            }

            return new List<CompletionItem>();
        }

        private bool IsMethodOverload(MethodDescriptor descriptor, IReadOnlyList<Expression> parameters)
        {
            // TODO:
            return descriptor.Parameters.Count == parameters.Count;
        }

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (BinaryExpression)expression;

            switch (casted.OperatorType)
            {
                case BinaryOperatorType.Assign:
                    if (casted.Left is IdentifierExpression id)
                    {
                        var isReadOnly = context.GetSymbols().Any(s => s.Name == id.Name && s.IsReadOnly);

                        if (isReadOnly)
                        {
                            return new Diagnostic
                            {
                                Position = id.Token.Column,
                                Length = id.Token.Text.Length,
                                Type = DiagnosticType.Error,
                                Message = $"'{id.Name}' is read-only."
                            }.ToOneItemArray();
                        }
                    }
                    break;
            }

            return new List<Diagnostic>();
        }

        public Type GetReturnType(Expression expression, IAnalysisContext context)
        {
            var casted = (BinaryExpression)expression;

            var type = AnalyzerService.GetReturnType(casted.Left, context);

            if (type != default)
            {
                switch (casted.OperatorType)
                {
                    case BinaryOperatorType.AccessMember:
                        if (casted.Right is IdentifierExpression property)
                        {
                            return TypeHelper.GetProperties(type)
                                .FirstOrDefault(p => !p.Descriptor.IsIndexer && p.Descriptor.Name == property.Name)
                                ?.Descriptor.ReturnType;
                        }
                        else if (casted.Right is InvocationExpression invocation)
                        {
                            return TypeHelper.GetMethods(type)
                                .FirstOrDefault(m => m.Descriptor.Name == invocation.MethodName && IsMethodOverload(m.Descriptor, invocation.Arguments.Items))
                                ?.Descriptor.ReturnType;
                        }
                        else if (casted.Right is IndexerExpression indexer)
                        {
                            var prop = TypeHelper.GetProperties(type)
                                .FirstOrDefault(p => !p.Descriptor.IsIndexer && p.Descriptor.Name == indexer.PropertyName);

                            var indexType = AnalyzerService.GetReturnType(indexer.Index, context);
                            return TypeHelper.GetProperties(prop.Descriptor.ReturnType)
                                .FirstOrDefault(p => p.Descriptor.IsIndexer && p.Descriptor.ParameterType == (indexType ?? p.Descriptor.ParameterType))
                                ?.Descriptor.ReturnType;
                        }
                        break;

                    case BinaryOperatorType.Assign:
                        break;

                    default:
                        var leftType = AnalyzerService.GetReturnType(casted.Left, context);
                        var rightType = AnalyzerService.GetReturnType(casted.Right, context);

                        if (leftType != default && rightType != default)
                        {
                            var op = TypeHelper.GetBinaryOperator(casted.OperatorType, leftType, rightType);
                            return op.ReturnType;
                        }

                        return default;
                }
            }

            return default;
        }
    }
}
