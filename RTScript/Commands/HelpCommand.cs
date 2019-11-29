namespace RTScript
{
    [ConsoleCommand("-h", "-?", "-help", Description = "Shows a list of commands")]
    public class HelpCommand : IConsoleCommand
    {
        public void Execute(RTScriptConsole console)
        {
            var commands = RTScriptConsole.Commands;

            foreach (var kvp in commands)
            {
                console.WriteLine($"{kvp.Key.Name}\t{kvp.Key.Description}");
            }
        }
    }
}
