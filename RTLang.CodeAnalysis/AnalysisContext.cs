using System;
using System.Collections.Generic;
using System.Linq;

namespace RTLang.CodeAnalysis
{
    public class AnalysisContext : IAnalysisContext, IExecutionContext
    {
        private readonly IExecutionContext _context;
        private readonly Dictionary<string, Type> _tempType = new Dictionary<string, Type>();
        private readonly HashSet<string> _definedSymbols = new HashSet<string>();
        private readonly List<Symbol> _temp = new List<Symbol>(8);
        private readonly List<Symbol> _symbols = new List<Symbol>(64)
        {
            new Symbol
            {
                Name  = "const",
                Type = SymbolType.Keyword,
                IsReadOnly = true
            },
            new Symbol
            {
                Name  = "null",
                Type = SymbolType.Keyword,
                IsReadOnly = true
            },
            new Symbol
            {
                Name  = "print",
                Type = SymbolType.Keyword,
                IsReadOnly = true
            },
            new Symbol
            {
                Name  = "false",
                Type = SymbolType.Keyword,
                IsReadOnly = true
            },
            new Symbol
            {
                Name  = "true",
                Type = SymbolType.Keyword,
                IsReadOnly = true
            },
            new Symbol
            {
                Name  = "var",
                Type = SymbolType.Keyword,
                IsReadOnly = true
            }
        };

        public AnalysisContext(IExecutionContext context)
            => _context = context ?? throw new AnalysisException($"'{nameof(context)}' is null.");

        #region Execution Context Decorator

        public void Assign(string name, object value)
            => _context.Assign(name, value);

        public void Declare(string name, object value, bool isConst = false)
        {
            _context.Declare(name, value, isConst);
            _definedSymbols.Add(name);
            _symbols.Add(new Symbol
            {
                IsReadOnly = isConst,
                Name = name,
                Type = SymbolType.Variable
            });
        }

        public void Declare(Type type)
        {
            _context.Declare(type);

            var name = type.ToFriendlyName();
            _definedSymbols.Add(name);
            _symbols.Add(new Symbol
            {
                IsReadOnly = true,
                Name = name,
                Type = SymbolType.Type
            });
        }

        public Type GetType(string name)
            => _context.GetType(name);

        public object Evaluate(LiteralType type, string value)
            => _context.Evaluate(type, value);

        public object Evaluate(UnaryOperatorType operatorType, object value)
            => _context.Evaluate(operatorType, value);

        public object Evaluate(BinaryOperatorType operatorType, object left, object right)
            => _context.Evaluate(operatorType, left, right);

        public object GetValue(string name)
            => _context.GetValue(name);

        public bool IsReadOnly(string name)
            => _context.IsReadOnly(name);

        public bool IsType(string name)
            => _context.IsType(name);

        public void Print(object value)
            => _context.Print(value);

        #endregion

        #region Analysis

        public Type GetLiteralType(LiteralType type, string value)
        {
            var result = _context.Evaluate(type, value);
            return result?.GetType();
        }

        public bool IsDefined(string name)
            => _definedSymbols.Contains(name);

        public bool IsMetadata(string name)
            => _tempType.ContainsKey(name);

        public Type GetSymbolType(string name)
        {
            if (_tempType.TryGetValue(name, out var value))
            {
                return value;
            }

            if (IsDefined(name))
            {
                return _context.GetType(name);
            }

            return default;
        }

        public IEnumerable<Symbol> GetSymbols()
            => _temp.Union(_symbols);

        public void AddMetadata(Symbol symbol, Type type = default)
        {
            _temp.Add(symbol);
            _tempType[symbol.Name] = type;
            _definedSymbols.Add(symbol.Name);
        }

        public void ClearMetadata()
        {
            for (int i = 0; i < _temp.Count; i++)
            {
                _definedSymbols.Remove(_temp[i].Name);
            }

            _tempType.Clear();
            _temp.Clear();
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

        #endregion
    }
}
