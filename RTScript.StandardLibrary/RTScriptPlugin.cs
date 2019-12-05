using RTLang;

namespace RTScript.StandardLibrary
{
    public static class RTScriptPlugin
    {
        public static void Inject(IExecutionContext context)
        {
            context.DeclareStatic<int>();
            context.DeclareStatic<float>();
            context.DeclareStatic<double>();
            context.DeclareStatic<bool>();
            context.DeclareStatic<char>();
            context.DeclareStatic<string>();
        }
    }
}
