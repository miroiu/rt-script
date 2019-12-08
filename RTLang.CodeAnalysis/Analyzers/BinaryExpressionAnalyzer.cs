using System;
using System.Collections.Generic;
using System.Linq;
using RTLang.Interpreter;
using RTLang.Parser;

namespace RTLang.CodeAnalysis.Analyzers
{
    [ExpressionEvaluator(typeof(BinaryExpression))]
    internal class BinaryExpressionAnalyzer : IExpressionAnalyzer
    {
        public IEnumerable<Completion> GetCompletions(Expression expression, IAnalysisContext context)
        {
            var casted = (BinaryExpression)expression;

            switch (casted.OperatorType)
            {
                case BinaryOperatorType.AccessMember:
                    var type = AnalyzerService.GetReturnType(casted.Left, context);

                    if (type != default)
                    {
                        // Can be both a property or an method completion
                        if (casted.Right is IdentifierExpression property)
                        {
                            return context.GetMembers(type)
                                .Where(s => s.Name != property.Name && s.Name.StartsWith(property.Name))
                                .Select(s => new Completion
                                {
                                    Text = s.Name,
                                    Type = s.Type
                                });
                        }

                        // This is when it is requesting arguments
                        if (casted.Right is InvocationExpression invocation)
                        {
                            return InvocationAnalyzer.GetMethodOverloadsCompletion(type, invocation.MethodName);
                        }
                    }

                    break;

                default:
                    return AnalyzerService.GetCompletions(casted.Right, context);
            }

            return Enumerable.Empty<Completion>();
        }

        public IEnumerable<Diagnostic> GetDiagnostics(Expression expression, IAnalysisContext context)
        {
            var casted = (BinaryExpression)expression;
            var left = casted.Left;
            var right = casted.Right;

            switch (casted.OperatorType)
            {
                case BinaryOperatorType.AccessMember:
                    var type = AnalyzerService.GetReturnType(left, context);

                    if (type != default)
                    {
                        if (right is IdentifierExpression property)
                        {
                            bool exists = context.GetMembers(type).Any(s => s.Name == property.Name && s.Type == SymbolType.Property);

                            if (!exists)
                            {
                                return new Diagnostic
                                {
                                    Position = property.Token.Position,
                                    Length = property.Token.Text.Length,
                                    Type = DiagnosticType.Error,
                                    Message = $"'{type.ToFriendlyName()}' does not have a property named: '{property.Name}'."
                                }.ToOneItemArray();
                            }
                        }
                        else if (right is InvocationExpression invocation)
                        {
                            bool exists = TypeHelper.GetMethods(type).Any(m => m.Descriptor.Name == invocation.MethodName && InvocationAnalyzer.IsMethodOverload(m.Descriptor, invocation.Arguments.Items));

                            if (!exists)
                            {
                                return new Diagnostic
                                {
                                    Position = invocation.Token.Position,
                                    Length = invocation.Token.Text.Length,
                                    Type = DiagnosticType.Error,
                                    Message = $"No matching overload found for '{invocation.MethodName}'"
                                }.ToOneItemArray();
                            }

                            return AnalyzerService.GetDiagnostics(invocation.Arguments, context);
                        }
                        else if (right is IndexerExpression indexer)
                        {
                            var prop = TypeHelper.GetProperties(type).FirstOrDefault(p => p.Descriptor.Name == indexer.PropertyName);

                            if (prop == default)
                            {
                                return new Diagnostic
                                {
                                    Position = right.Token.Position,
                                    Length = right.Token.Text.Length,
                                    Type = DiagnosticType.Error,
                                    Message = $"'{type.ToFriendlyName()}' does not have a property named '{indexer.PropertyName}'."
                                }.ToOneItemArray();
                            }

                            var indexType = AnalyzerService.GetReturnType(indexer.Index, context);
                            if (indexType != default)
                            {
                                var propType = prop.Descriptor.ReturnType;
                                if (!TypeHelper.GetProperties(propType).Any(p => p.Descriptor.IsIndexer && p.Descriptor.ParameterType == indexType))
                                {
                                    var index = indexer.Index;
                                    return new Diagnostic
                                    {
                                        Position = index.Token.Position,
                                        Length = index.Token.Text.Length,
                                        Type = DiagnosticType.Error,
                                        Message = $"'{propType.ToFriendlyName()}' does not have an index taking a '{indexType.ToFriendlyName()}' parameter."
                                    }.ToOneItemArray();
                                }
                            }

                            return AnalyzerService.GetDiagnostics(indexer.Index, context);
                        }
                    }

                    break;

                case BinaryOperatorType.Assign:
                    if (left is IdentifierExpression id)
                    {
                        var isReadOnly = context.GetSymbols().Any(s => s.Name == id.Name && s.IsReadOnly);

                        if (isReadOnly)
                        {
                            return new Diagnostic
                            {
                                Position = id.Token.Position,
                                Length = id.Token.Text.Length,
                                Type = DiagnosticType.Error,
                                Message = $"'{id.Name}' is read-only."
                            }.ToOneItemArray();
                        }
                    }
                    else if (left is BinaryExpression accessor)
                    {
                        var propertyType = AnalyzerService.GetReturnType(accessor, context);
                        if (propertyType != default)
                        {
                            if (accessor.Right is IdentifierExpression property)
                            {
                                var isReadOnly = context.GetMembers(propertyType).Any(s => s.Name == property.Name && s.Type == SymbolType.Property && s.IsReadOnly == true);
                                if (isReadOnly)
                                {
                                    return new Diagnostic
                                    {
                                        Position = property.Token.Position,
                                        Length = property.Token.Text.Length,
                                        Type = DiagnosticType.Error,
                                        Message = $"Property '{property.Name}' is read-only."
                                    }.ToOneItemArray();
                                }
                                else
                                {
                                    var initializerType = AnalyzerService.GetReturnType(casted.Right, context);
                                    if (initializerType != default)
                                    {
                                        if (!TypeHelper.CanChangeType(initializerType, propertyType))
                                        {
                                            return new Diagnostic
                                            {
                                                Position = property.Token.Position,
                                                Length = property.Token.Text.Length,
                                                Type = DiagnosticType.Error,
                                                Message = $"Cannot assign value of type '{initializerType.ToFriendlyName()}' to property of type '{propertyType.ToFriendlyName()}'"
                                            }.ToOneItemArray();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    break;

                default:
                    var leftDiagnostics = AnalyzerService.GetDiagnostics(left, context);
                    var rightDiagnostics = AnalyzerService.GetDiagnostics(right, context);
                    return leftDiagnostics.Union(rightDiagnostics);
            }

            return AnalyzerService.GetDiagnostics(left, context);
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
                                .FirstOrDefault(m => m.Descriptor.Name == invocation.MethodName && InvocationAnalyzer.IsMethodOverload(m.Descriptor, invocation.Arguments.Items))
                                ?.Descriptor.ReturnType;
                        }
                        else if (casted.Right is IndexerExpression indexer)
                        {
                            var prop = TypeHelper.GetProperties(type)
                                .FirstOrDefault(p => !p.Descriptor.IsIndexer && p.Descriptor.Name == indexer.PropertyName);

                            var indexType = AnalyzerService.GetReturnType(indexer.Index, context);
                            if (indexType != default)
                            {
                                return TypeHelper.GetProperties(prop.Descriptor.ReturnType)
                                    .FirstOrDefault(p => p.Descriptor.IsIndexer && p.Descriptor.ParameterType == indexType)
                                    ?.Descriptor.ReturnType;
                            }

                            return default;
                        }
                        break;

                    case BinaryOperatorType.Assign:
                        return type;

                    default:
                        var leftType = AnalyzerService.GetReturnType(casted.Left, context);
                        var rightType = AnalyzerService.GetReturnType(casted.Right, context);

                        if (leftType != default && rightType != default)
                        {
                            var op = TypeHelper.GetBinaryOperator(casted.OperatorType, leftType, rightType);
                            return op?.ReturnType;
                        }

                        return default;
                }
            }

            return default;
        }
    }
}
