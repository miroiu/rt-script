using RTScript.Language.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTScript.Language.Completion
{
    public class CompletionContext : ICompletionContext
    {
        private readonly IExecutionContext _context;

        public CompletionContext(IExecutionContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetSymbolCompletion(string name)
        {
            var symbols = _context.GetSymbols();

            return symbols.Where(s => s.StartsWith(name));
        }

        public Type GetSymbolType(string name)
        {
            var symbols = _context.GetSymbols();

            var symbol = symbols.FirstOrDefault(s => s == name);

            if (symbol != default)
            {
                return _context.GetType(symbol);
            }

            return default;
        }
    }
}