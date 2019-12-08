using System;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    public interface IAnalysisContext
    {
        Type GetSymbolType(string name);

        Type GetLiteralType(LiteralType type, string value);

        // Returns all the members of a type
        IEnumerable<Symbol> GetMembers(Type type);

        // Returns keywords, variables and types
        IEnumerable<Symbol> GetSymbols();

        // Tells if there is a symbol with this name
        bool IsDefined(string name);

        // Tells if there is a temporary symbol with this name
        bool IsMetadata(string name);

        // Adds a temporary symbol (e.g. variable defined in a previous statement that was not evaluated)
        void AddMetadata(Symbol symbol, Type type = default);

        // Clears the temporary symbols
        void ClearMetadata();
    }
}