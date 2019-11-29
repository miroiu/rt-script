using RTScript.Language.Interpreter;

namespace RTScript.Language.Completion
{
    public class RTScriptCompletion
    {
        private readonly IExecutionContext _context;

        public RTScriptCompletion(IExecutionContext context)
        {
            _context = context;
        }
    }
}
