using System;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Startup;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace Arbor.AppModel.HealthChecks;

[UsedImplicitly]
public class HealthBackgroundService(
    HealthChecker? healthChecker = null,
    StartupTaskContext? startupTaskContext = null)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        if (healthChecker is null || startupTaskContext is null)
        {
            return;
        }

        try
        {
            while (!startupTaskContext.IsCompleted)
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }

            await healthChecker.PerformHealthChecksAsync(stoppingToken);
        }
        catch (TaskCanceledException)
        {
            //
        }
    }
}