namespace RTScript.Commands
{
    [ConsoleCommand("-v", Description = "shows the repl version")]
    public sealed class VersionCommand : IConsoleCommand
    {
        public const string Version = "1.0";

        public void Execute(RTScriptConsole console)
        {
            console.WriteLine(Version);
        }
    }
}
