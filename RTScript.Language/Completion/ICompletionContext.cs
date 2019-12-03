﻿using System.Collections.Generic;

namespace RTScript.Language.Completion
{
    public interface ICompletionContext
    {
        IEnumerable<string> GetSymbolCompletion(string name);
    }
}