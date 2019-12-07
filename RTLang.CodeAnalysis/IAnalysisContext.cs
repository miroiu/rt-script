﻿using System;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    internal interface IAnalysisContext
    {
        Type GetType(string variableName);

        // Returns all the members of a type
        IEnumerable<Symbol> GetMembers(Type type);

        // Returns keywords, variables and types
        IEnumerable<Symbol> GetSymbols();

        // Returns a keyword, variable or a type
        Symbol GetSymbol(string name);

        // Returns all the type symbols
        IEnumerable<Symbol> GetTypes();

        // Returns all variable symbols
        IEnumerable<Symbol> GetVariables();

        Type GetLiteralType(LiteralType type, string value);

        // Adds a temporary symbol (e.g. variable defined in a previous statement that was not evaluated)
        void AddMetadata(SymbolMetadata symbol);

        // Clears the temporary symbols
        void ClearMetadata();
    }
}