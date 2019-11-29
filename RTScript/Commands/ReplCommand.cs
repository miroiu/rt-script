namespace RTScript
{
    [ConsoleCommand("-r", "-repl", Description = "read-eval-print-loop")]
    public class ReplCommand : IConsoleCommand
    {
        private RTScriptRepl _repl;

        public void Execute(RTScriptConsole console)
        {
            _repl = new RTScriptRepl(console);

            while (true)
            {
                var line = console.ReadLine();
                _repl.Evaluate(line);
            }
        }
    }
}
