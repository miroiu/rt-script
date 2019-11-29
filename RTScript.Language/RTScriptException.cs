using System;

namespace RTScript.Language
{
    public abstract class RTScriptException : Exception
    {
        public RTScriptException(string message) : base(message)
        {
        }
    }
}
