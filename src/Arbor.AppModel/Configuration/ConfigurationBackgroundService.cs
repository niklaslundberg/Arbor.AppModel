using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace Arbor.AppModel.Configuration;

[UsedImplicitly]
public class ConfigurationBackgroundService(UserConfigUpdater updater) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        updater.Start();

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken).ConfigureAwait(false);
        }
    }
}