using RTLang;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RTScript
{
    public class RTScriptConsole : IOutputStream
    {
        #region Command loading

        public static readonly Dictionary<CommandInfo, IConsoleCommand> Commands = typeof(IConsoleCommand).Assembly.GetTypes()
            .Where(t => typeof(IConsoleCommand).IsAssignableFrom(t) && t.CustomAttributes.Any())
            .Select(t => new
            {
                Info = GetCommandInfo(t),
                Type = t
            })
            .ToDictionary(x => x.Info, x => (IConsoleCommand)Activator.CreateInstance(x.Type));

        private static CommandInfo GetCommandInfo(Type type)
        {
            ConsoleCommandAttribute cmdAttr = default;
            List<CommandOptionAttribute> options = new List<CommandOptionAttribute>();

            var attributes = type.GetCustomAttributes(false);

            foreach (var attr in attributes)
            {
                if (attr is ConsoleCommandAttribute cAttr)
                {
                    cmdAttr = cAttr;
                }
                else if (attr is CommandOptionAttribute oAttr)
                {
                    options.Add(oAttr);
                }
            }

            var result = new CommandInfo(cmdAttr.Name, cmdAttr.Description, options);
            return result;
        }

        #endregion

        private readonly string[] _arguments;

        public RTScriptConsole(string[] arguments)
            => _arguments = arguments;

        public void WriteLine(string line = default)
            => Console.WriteLine(line);

        public void Clear()
            => Console.Clear();

        public string ReadLine()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }

        public void Write(string value)
            => Console.Write(value);

        public void RunCommand(string cmd)
        {
            if (Commands.TryGetValue(new CommandInfo(cmd), out var command))
            {
                command.Execute(this);
            }
            else if (Commands.TryGetValue(new CommandInfo("-?"), out var helpCmd))
            {
                helpCmd.Execute(this);
            }
        }

        public string[] GetArguments()
            => _arguments;

        public string[] GetOptions(string command)
        {
            var arguments = GetArguments();
            List<string> args = new List<string>();

            for (var i = 0; i < arguments.Length; i++)
            {
                var next = i + 1;
                if (arguments[i] == command && next < arguments.Length)
                {
                    for (i = next; i < arguments.Length; i++)
                    {
                        var arg = arguments[i];
                        if (arg.StartsWith("-"))
                        {
                            return args.ToArray();
                        }

                        args.Add(arg);
                    }
                }
            }

            return args.ToArray();
        }
    }
}
