using RTScript;
using System.Text;

namespace RTScript.Tests.Mocks
{
    public class MockOutputStream : IOutputStream
    {
        private readonly StringBuilder _output = new StringBuilder();

        public void WriteLine(string line)
            => _output.AppendLine(line);

        public void Clear()
            => _output.Clear();

        public string Output
            => _output.ToString();
    }
}
