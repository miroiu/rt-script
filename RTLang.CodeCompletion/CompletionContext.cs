using RTLang.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeCompletion
{
    public class CompletionContext : ICompletionContext
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

        public CompletionContext(IExecutionContext context)
            => _context = context ?? throw new CompletionException($"'{nameof(context)}' is null.");

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