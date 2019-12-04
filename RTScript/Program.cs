using System;
using System.Linq;

namespace RTScript
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            // TODO: Load assembly?
            if (args.Length != 0)
            {
                var console = new RTScriptConsole(args.Skip(1).ToArray());
                console.RunCommand(args[0]);
            }
            else
            {
                var console = new RTScriptConsole(args.ToArray());
                console.RunCommand("-?");
            }
        }
    }
}
