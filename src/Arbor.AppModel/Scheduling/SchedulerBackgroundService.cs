using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Arbor.AppModel.Scheduling
{
    [UsedImplicitly]
    public class SchedulerBackgroundService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly ITimer _timer;
        private readonly IScheduler _scheduler;
        private readonly ImmutableArray<ScheduledService> _services;

        public SchedulerBackgroundService(ITimer timer,
            IScheduler scheduler,
            IEnumerable<ScheduledService> services,
            ILogger logger)
        {
            _timer = timer;

            _scheduler = scheduler;
            _services = services.ToImmutableArray();
            _logger = logger;

            foreach (var scheduledService in _services)
            {
                _scheduler.Add(scheduledService, scheduledService.RunAsync);
            }

            _timer.Register(scheduler);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            _logger.Information("Found {@Schedules} schedules", _services.Select(service => service.Name).ToArray());
            _logger.Information("Running scheduler {Scheduler}", _scheduler);

            await _timer.Run(stoppingToken);
        }
    }
}