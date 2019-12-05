namespace RTScript
{
    [CommandOption("--add", Arguments = "<file>.dll", Description = "Loads one or multiple plugin files.")]
    [ConsoleCommand("-r", Description = "starts in repl mode")]
    public sealed class ReplCommand : IConsoleCommand
    {
        private RTScriptRepl _repl;

        public void Execute(RTScriptConsole console)
        {
            _repl = new RTScriptRepl(console);

            var plugins = console.GetOptions("--add");
            foreach (var plugin in plugins)
            {
                _repl.AddReference(plugin);
            }

            while (true)
            {
                var line = console.ReadLine();
                _repl.Evaluate(line);
            }
        }
    }
}
