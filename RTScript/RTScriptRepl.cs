using RTLang;

namespace RTScript
{
    public class RTScriptRepl
    {
        public RTScriptConsole Console;
        private readonly IExecutionContext _context;

        public RTScriptRepl(RTScriptConsole console)
        {
            Console = console;
            _context = new ExecutionContext(console);

            _context.Declare("int", 0, true);
            _context.Declare("float", 0.0f, true);
            _context.Declare("double", 0.0, true);
            _context.Declare("bool", true, true);
            _context.Declare("char", ' ', true);
            _context.Declare("string", string.Empty, true);
        }

        public void Evaluate(string code)
            => RTLang.RTScript.Execute(code, _context);
    }
}
