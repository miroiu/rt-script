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

        public IEnumerable<string> GetMembers(Type type)
        {
            var props = TypesCache.GetProperties(type).Select(p => p.Descriptor.Name);
            var methods = TypesCache.GetMethods(type).Select(m => m.Descriptor.Name);

            return props.Union(methods);
        }

        public IEnumerable<string> GetSymbols()
            => _keywords.Union(_context.GetSymbols());

        //public IEnumerable<string> GetMethodArguments(Type type, string methodName)
        //{
        //}
    }
}