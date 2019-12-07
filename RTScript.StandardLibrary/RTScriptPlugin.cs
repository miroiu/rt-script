using RTLang;

namespace RTScript.StandardLibrary
{
    public static class RTScriptPlugin
    {
        public static void Inject(IExecutionContext context)
        {
            context.Declare<int>();
            context.Declare<float>();
            context.Declare<double>();
            context.Declare<bool>();
            context.Declare<char>();
            context.Declare<string>();
        }
    }
}
