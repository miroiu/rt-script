using System.Collections.Generic;
using System.IO;

namespace RTScript
{
    [CommandOption("--add", Arguments = "<file>.dll", Description = "loads one or multiple plugin files.")]
    [ConsoleCommand("-f", Description = "execute .rt files")]
    public sealed class FileCommand : IConsoleCommand
    {
        private RTScriptRepl _repl;
        private const string _extension = ".rt";

        public void Execute(RTScriptConsole console)
        {
            _repl = new RTScriptRepl(console);

            var files = console.GetOptions("-f");
            var plugins = console.GetOptions("--add");

            foreach (var plugin in plugins)
            {
                _repl.AddReference(plugin);
            }

            var sources = new List<string>();
            var missingFiles = new List<string>();

            foreach (var file in files)
            {
                if (Path.GetExtension(file) == _extension && File.Exists(file))
                {
                    var source = File.ReadAllText(file);
                    sources.Add(source);
                }
                else
                {
                    missingFiles.Add(file);
                }
            }

            if (missingFiles.Count > 0)
            {
                console.WriteLine($"Could not load: {string.Join(", ", missingFiles)}");

                if (missingFiles.Count != files.Length)
                {
                    console.WriteLine($"Do you want to continue? (y/n)");
                    var answer = console.ReadLine().ToLower();

                    if (answer == "n" || answer == "no")
                    {
                        return;
                    }
                }
            }

            foreach (var source in sources)
            {
                _repl.Evaluate(source);
            }
        }
    }
}
