using RTLang;
using System.Text;

namespace RTScript.Editor
{
    public class OutputStream : IOutputStream
    {
        private readonly StringBuilder _result = new StringBuilder(64);

        public void Clear()
            => _result.Clear();

        public void WriteLine(string line)
            => _result.AppendLine(line);

        public string Output
            => _result.ToString();
    }
}
