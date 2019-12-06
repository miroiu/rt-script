using RTLang.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeAnalysis
{
    internal class AnalysisContext : IAnalysisContext
    {
        private readonly IExecutionContext _context;
        private readonly string[] _keywords = new string[]
        {
            "const",
            "null",
            "print",
            "false",
            "true",
            "var"
        };

        public AnalysisContext(IExecutionContext context)
            => _context = context ?? throw new AnalysisException($"'{nameof(context)}' is null.");

        public Type GetType(string variable)
            => _context.GetType(variable);

        public IEnumerable<Symbol> GetMembers(Type type)
        {
            var props = TypesCache.GetProperties(type)
                .Select(p => new Symbol
                {
                    Type = SymbolType.Property,
                    IsReadOnly = !p.Descriptor.CanWrite,
                    Name = p.Descriptor.Name
                });

            var methods = TypesCache.GetMethods(type)
                .Select(p => new Symbol
                {
                    Type = SymbolType.Method,
                    IsReadOnly = true,
                    Name = p.Descriptor.Name
                });

            return props.Union(methods);
        }

        public IEnumerable<Symbol> GetSymbols()
            => _context.GetTypes().Select(s => new Symbol
            {
                IsReadOnly = true,
                Name = s,
                Type = SymbolType.Type
            }).Union(_context.GetVariables().Select(s => new Symbol
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

        public IEnumerable<Symbol> GetTypes()
            => _context.GetTypes().Select(t => new Symbol
            {
                IsReadOnly = true,
                Name = t,
                Type = SymbolType.Type
            });

        public IEnumerable<Symbol> GetVariables()
            => _context.GetTypes().Select(t => new Symbol
            {
                IsReadOnly = _context.IsReadOnly(t),
                Name = t,
                Type = SymbolType.Variable
            });

        //public IEnumerable<string> GetMethodArguments(Type type, string methodName)
        //{
        //}
    }
}