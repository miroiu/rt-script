using System;

namespace RTScript.Language.Interpreter.Interop
{
    [Flags]
    public enum PropertyFlag
    {
        Read,
        Write,
        ReadWrite = Read & Write
    }
}
