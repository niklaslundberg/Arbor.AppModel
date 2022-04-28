using Arbor.AppModel.Application;
using Microsoft.AspNetCore.Builder;

namespace Arbor.AppModel.Logging
{
    public static class VerboseLoggingExtensions
    {
        public static IApplicationBuilder AddRequestLogging(this IApplicationBuilder app,
            EnvironmentConfiguration environmentConfiguration)
        {
            if (environmentConfiguration.UseVerboseLogging)
            {
                return app.UseMiddleware<VerboseLoggingMiddleware>();
            }

            return app;
        }
    }
}