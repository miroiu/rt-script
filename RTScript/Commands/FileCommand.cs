using System.IO;

namespace RTScript
{
    [ConsoleCommand("-f", "-file", Description = "execute .rt file")]
    public class FileCommand : IConsoleCommand
    {
        private RTScriptRepl _repl;
        private const string _extension = ".rt";

        public void Execute(RTScriptConsole console)
        {
            _repl = new RTScriptRepl(console);

            var arguments = console.GetArguments();
            var path = Path.Combine(Directory.GetCurrentDirectory(), arguments[0]);

            if (Path.GetExtension(arguments[0]) == _extension && File.Exists(path))
            {
                var source = File.ReadAllText(path);
                _repl.Evaluate(source);
            }
            else
            {
                console.WriteLine($"File {arguments[0]} not found.");
            }
        }
    }
}
