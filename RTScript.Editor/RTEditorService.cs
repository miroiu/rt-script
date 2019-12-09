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

        public static void Init(IServiceProvider services)
        {
            _langService = services.GetService<RTLangService>();
            _execContext = services.GetService<IExecutionContext>();
        }

        public static IReadOnlyList<Completion> GetCompletions(string code, int position)
            => _langService.GetCompletions(code, position);

        public static IReadOnlyList<Diagnostic> GetDiagnostics(string code)
            => _langService.GetDiagnostics(code);

        public static void Execute(string code)
            => RTLang.RTScript.Execute(code, _execContext);
    }
}
