using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using Serilog.Events;

namespace Arbor.AppModel.Logging;

public class VerboseLoggingMiddleware(ILogger logger, RequestDelegate next)
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [UsedImplicitly]
    public async Task InvokeAsync(HttpContext context)
    {
        bool loggingEnabled = _logger.IsEnabled(LogEventLevel.Verbose);

        string? commonRequestInfo = null;

        if (loggingEnabled)
        {
            commonRequestInfo =
                $"{context.Request.GetDisplayUrl()} from remote IP {context.Connection.RemoteIpAddress}";

            _logger.Verbose("Starting request {RequestInfo}", commonRequestInfo);
        }

        await next.Invoke(context).ConfigureAwait(false);

        if (loggingEnabled)
        {
            _logger.Verbose("Ending request {RequestInfo}, status code {StatusCode}",
                commonRequestInfo,
                context.Response.StatusCode);
        }
    }
}