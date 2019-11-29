namespace RTScript.Commands
{
    [ConsoleCommand("-v", "-version", Description = "Shows the repl version")]
    public class VersionCommand : IConsoleCommand
    {
        public const string Version = "2.0";

        public void Execute(RTScriptConsole console)
        {
            console.WriteLine(Version);
        }
    }
}
