using System;

namespace RTLang.CodeCompletion
{
    public class CompletionException : Exception
    {
        public CompletionException(string message) : base(message)
        {
        }
    }
}
