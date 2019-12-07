using System;
using System.Collections.Generic;
using System.Linq;
using RTLang.Interop;
using RTLang.Interpreter;
using RTLang.Parser;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(BinaryExpression))]
    internal class BinaryExpressionAnalyzer : IExpressionAnalyzer
    {
        public IEnumerable<CompletionItem> GetCompletions(Expression expression, IAnalysisContext context)
        {
            var casted = (BinaryExpression)expression;

            switch (casted.OperatorType)
            {
                case BinaryOperatorType.AccessMember:
                    var type = GetExpressionReturnType(casted.Left, context);

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

        private Type GetExpressionReturnType(Expression left, IAnalysisContext context)
        {
            if (left is IdentifierExpression id)
            {
                return context.GetType(id.Name);
            }

            if (left is UnaryExpression unary)
            {
                // TODO:
            }

            if (left is BinaryExpression bExp)
            {
                var type = GetExpressionReturnType(bExp.Left, context);

                switch (bExp.OperatorType)
                {
                    case BinaryOperatorType.AccessMember:
                        if (bExp.Right is IdentifierExpression property)
                        {
                            return TypeHelper.GetProperties(type)
                                .FirstOrDefault(p => !p.Descriptor.IsIndexer && p.Descriptor.Name == property.Name)
                                ?.Descriptor.ReturnType;
                        }
                        else if (bExp.Right is InvocationExpression invocation)
                        {
                            return TypeHelper.GetMethods(type)
                                .FirstOrDefault(m => m.Descriptor.Name == invocation.MethodName && IsMethodOverload(m.Descriptor, invocation.Arguments.Items))
                                ?.Descriptor.ReturnType;
                        }
                        else if (bExp.Right is IndexerExpression indexer)
                        {
                            return TypeHelper.GetProperties(type)
                                .FirstOrDefault(p => p.Descriptor.IsIndexer && p.Descriptor.Name == indexer.PropertyName && p.Descriptor.ParameterType == GetExpressionReturnType(indexer.Index, context))
                                ?.Descriptor.ReturnType;
                        }
                        break;

                    case BinaryOperatorType.Assign:
                        break;

                    default:
                        var leftType = GetExpressionReturnType(bExp.Left, context);
                        var rightType = GetExpressionReturnType(bExp.Right, context);
                        var op = TypeHelper.GetBinaryOperator(bExp.OperatorType, leftType, rightType);
                        return op.ReturnType;
                }

            }

            // else
            // TODO: visit expression (TypeVisitor + IExpressionEvaluator which returns a type?)

            return default;
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
    }
}
