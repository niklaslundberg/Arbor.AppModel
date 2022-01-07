using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Arbor.AppModel.Scheduling;

[UsedImplicitly]
public class DelayBackgroundService : BackgroundService
{
    private readonly ILogger _logger;

    public DelayBackgroundService(ILogger logger) => this._logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(10));

            _logger.Information("Delayed from background service");
        }
    }
}