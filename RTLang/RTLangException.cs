using System;

namespace RTLang
{
    public abstract class RTLangException : Exception
    {
        public RTLangException(string message) : base(message)
        {
        }
    }
}
