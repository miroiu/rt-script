using RTLang;
using RTLang.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace RTScript.Editor
{
    public static class RTEditorService
    {
        private static RTLangService _langService;
        private static IExecutionContext _execContext;
        private static OutputStream _output;

        public static void Init(IServiceProvider services)
        {
            _langService = services.GetService<RTLangService>();
            _execContext = services.GetService<IExecutionContext>();
            _output = services.GetService<OutputStream>();
        }

        public static IReadOnlyList<Completion> GetCompletions(string code, int position)
            => _langService.GetCompletions(code, position);

        public static IReadOnlyList<Diagnostic> GetDiagnostics(string code)
            => _langService.GetDiagnostics(code);

        public static ExecutionResult Execute(string code)
        {
            RTLang.RTScript.Execute(code, _execContext);

            var result = new ExecutionResult
            {
                Output = _output.Output
            };

            _output.Clear();
            return result;
        }
    }
}
