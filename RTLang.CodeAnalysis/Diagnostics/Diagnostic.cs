namespace RTLang.CodeAnalysis
{
    public struct Diagnostic
    {
        public DiagnosticType Type;
        public int Position;
        public int Length;
        public string Message;
    }
}