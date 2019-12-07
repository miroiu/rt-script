using System;

namespace RTLang.CodeAnalysis
{
    internal class SymbolMetadata
    {
        public SymbolMetadata(Symbol symbol, Type type)
        {
            Symbol = symbol;
            Type = type;
        }

        public Symbol Symbol { get; }
        public Type Type { get; }
    }
}