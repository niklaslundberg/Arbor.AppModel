using System;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Scheduling;
using Serilog;

namespace Arbor.AppModel.Tests;

public class DelayWithSkipBehaviorScheduledService : TestScheduledService
{
    private readonly TimeSpan _delay;
    private readonly ILogger _logger;

    public DelayWithSkipBehaviorScheduledService(TimeSpan delay, ISchedule schedule, IScheduler scheduler, ILogger logger) : base(schedule,
        scheduler)
    {
        _delay = delay;
        _logger = logger;
        SchedulingOptions = new SchedulingOptions(schedulingBehavior: SchedulingBehavior.Skip);
    }

    public override async Task RunAsync(DateTimeOffset currentTime, CancellationToken stoppingToken)
    {
        _logger.Debug("Executing DelayAllowOverdueScheduledService");
        Invokations++;
        await Task.Delay(_delay, stoppingToken);
        _logger.Debug("Executed DelayAllowOverdueScheduledService");
        CompletedInvokations++;
    }
}