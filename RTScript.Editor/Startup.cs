using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using RTLang;
using RTLang.CodeAnalysis;

namespace RTScript.Editor
{
    public class Startup
    {
        private readonly AnalysisContext _analysisContext;
        private readonly RTLangService _langService;

        public Startup()
        {
            var executionContext = RTLang.RTScript.NewContext(new OutputStream());
            _analysisContext = RTLangService.NewContext(executionContext);
            _langService = RTLangService.Create(_analysisContext);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAnalysisContext>(_analysisContext);
            services.AddSingleton<IExecutionContext>(_analysisContext);
            services.AddSingleton(_langService);
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            RTEditorService.Init(app.Services);
        }
    }
}
