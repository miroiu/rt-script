using RTLang;
using System;

namespace RTScript.Editor
{
    public class OutputStream : IOutputStream
    {
        public void Clear()
            => Console.Clear();

        public void WriteLine(string line)
            => Console.WriteLine(line);
    }
}
