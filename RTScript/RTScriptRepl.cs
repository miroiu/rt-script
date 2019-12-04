using RTLang;

namespace RTScript
{
    public class RTScriptRepl
    {
        private readonly IExecutionContext _context;

        public RTScriptRepl(IOutputStream output)
        {
            _context = RTLang.RTScript.NewContext(output);

            _context.DeclareStatic<int>();
            _context.DeclareStatic<float>();
            _context.DeclareStatic<double>();
            _context.DeclareStatic<bool>();
            _context.DeclareStatic<char>();
            _context.DeclareStatic<string>();
        }

        public void Evaluate(string code)
            => RTLang.RTScript.Execute(code, _context);
    }
}
