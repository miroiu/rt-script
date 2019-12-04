using RTLang;

namespace RTScript
{
    public class RTScriptRepl
    {
        private readonly IExecutionContext _context;

        public RTScriptRepl(IOutputStream output)
        {
            _context = RTLang.RTScript.NewContext(output);

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
