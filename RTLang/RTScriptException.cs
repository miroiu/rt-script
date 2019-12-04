using System;

namespace RTScript
{
    public abstract class RTScriptException : Exception
    {
        public RTScriptException(string message) : base(message)
        {
        }
    }
}
