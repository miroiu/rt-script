using System;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    internal interface IAnalysisContext
    {
        Type GetType(string variableName);

        // Returns all the members of a
        IEnumerable<Symbol> GetMembers(Type type);

        // Returns variables and static types
        IEnumerable<Symbol> GetSymbols();

        IEnumerable<Symbol> GetTypes();

        IEnumerable<Symbol> GetVariables();
    }
}