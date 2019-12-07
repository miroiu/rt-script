using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeAnalysis
{
    internal class AnalysisContext : IAnalysisContext
    {
        private readonly IExecutionContext _context;
        private readonly HashSet<string> _keywords = new HashSet<string>
        {
            "const",
            "null",
            "print",
            "false",
            "true",
            "var"
        };

        private readonly Dictionary<string, SymbolMetadata> _temporary = new Dictionary<string, SymbolMetadata>();

        public AnalysisContext(IExecutionContext context)
            => _context = context ?? throw new AnalysisException($"'{nameof(context)}' is null.");

        public Type GetType(string variable)
        {
            try
            {
                if (_temporary.TryGetValue(variable, out var value))
                {
                    return value.Type;
                }

                return _context.GetType(variable);
            }
            catch
            {
                return default;
            }
        }

        public IEnumerable<Symbol> GetMembers(Type type)
        {
            var props = TypeHelper.GetProperties(type)
                .Where(p => !p.Descriptor.IsIndexer)
                .Select(p => new Symbol
                {
                    Type = SymbolType.Property,
                    IsReadOnly = !p.Descriptor.CanWrite,
                    Name = p.Descriptor.Name
                });

            var methods = TypeHelper.GetMethods(type)
                .Select(p => new Symbol
                {
                    Type = SymbolType.Method,
                    IsReadOnly = true,
                    Name = p.Descriptor.Name
                });

            return props.Union(methods);
        }

        public IEnumerable<Symbol> GetSymbols()
            => _temporary.Values
            .Select(v => v.Symbol)
            .Union(_context.GetTypes().Select(s => new Symbol
            {
                IsReadOnly = true,
                Name = s,
                Type = SymbolType.Type
            })).Union(_context.GetVariables().Select(s => new Symbol
            {
                IsReadOnly = _context.IsReadOnly(s),
                Name = s,
                Type = SymbolType.Variable
            })).Union(_keywords.Select(s => new Symbol
            {
                IsReadOnly = true,
                Name = s,
                Type = SymbolType.Keyword
            }));

        public Symbol GetSymbol(string name)
        {
            try
            {
                if (_temporary.TryGetValue(name, out var temp))
                {
                    return temp.Symbol;
                }

                return _keywords.Contains(name) ? new Symbol
                {
                    IsReadOnly = true,
                    Name = name,
                    Type = SymbolType.Keyword
                } : _context.IsType(name) ? new Symbol
                {
                    IsReadOnly = true,
                    Name = name,
                    Type = SymbolType.Type
                } : new Symbol
                {
                    IsReadOnly = _context.IsReadOnly(name),
                    Name = name,
                    Type = SymbolType.Variable
                };
            }
            catch
            {
                return default;
            }
        }

        public IEnumerable<Symbol> GetTypes()
            => _temporary
            .Values
            .Where(s => s.Symbol.Type == SymbolType.Type)
            .Select(s => s.Symbol)
            .Union(_context.GetTypes().Select(t => new Symbol
            {
                IsReadOnly = true,
                Name = t,
                Type = SymbolType.Type
            }));

        public IEnumerable<Symbol> GetVariables()
            => _temporary
            .Values
            .Where(s => s.Symbol.Type == SymbolType.Variable)
            .Select(s => s.Symbol)
            .Union(_context.GetTypes().Select(t => new Symbol
            {
                IsReadOnly = _context.IsReadOnly(t),
                Name = t,
                Type = SymbolType.Variable
            }));

        public void AddMetadata(SymbolMetadata symbol)
            => _temporary.Add(symbol.Symbol.Name, symbol);

        public void ClearMetadata()
            => _temporary.Clear();

        public Type GetLiteralType(LiteralType type, string value)
        {
            var result = _context.Evaluate(type, value);
            return result?.GetType();
        }

        //public IEnumerable<MethodOverloadDescription> GetMethodOverloads(Type type, string methodName)
        //{
        //}
    }
}