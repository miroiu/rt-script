using System.Collections.Generic;
using System.Text;

namespace RTScript
{
    [ConsoleCommand("-?", Description = "shows a list of commands")]
    public sealed class HelpCommand : IConsoleCommand
    {
        public void Execute(RTScriptConsole console)
        {
            var commands = RTScriptConsole.Commands;
            Dictionary<string, CommandOptionAttribute> options = new Dictionary<string, CommandOptionAttribute>();

            console.WriteLine("Commands:");
            foreach (var info in commands.Keys)
            {
                console.WriteLine(CreateColumns(20, info.Name, info.Description));

                foreach (var option in info.Options)
                {
                    options[option.Name] = option;
                }
            }

            console.WriteLine();
            console.WriteLine("Options:");
            foreach (var kvp in options)
            {
                var option = kvp.Value;
                console.WriteLine(CreateColumns(20, $"{option.Name} {option.Arguments}", option.Description));
            }
        }

        public static string CreateColumns(int spacing, params object[] columns)
        {
            StringBuilder builder = new StringBuilder(columns.Length * spacing);

            for (int i = 0; i < columns.Length; i++)
            {
                builder.Append($"{{{i},{-spacing}}}");
            }

            return string.Format(builder.ToString(), columns);
        }
    }
}
