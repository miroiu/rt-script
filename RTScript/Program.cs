using System;
using System.Linq;

namespace RTScript
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                var console = new RTScriptConsole(args);
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
