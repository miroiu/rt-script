using System;
using System.Collections.Generic;

namespace RTLang.CodeAnalysis
{
    internal interface IAnalysisContext
    {
        Type GetType(string variableName);

        IEnumerable<string> GetMembers(Type type);

        IEnumerable<string> GetSymbols();
    }
}