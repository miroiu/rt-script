using System;
using System.Collections.Generic;

namespace RTLang.CodeCompletion
{
    public interface ICompletionContext
    {
        Type GetType(string variableName);

        IEnumerable<string> GetMembers(Type type);

        IEnumerable<string> GetSymbols();
    }
}