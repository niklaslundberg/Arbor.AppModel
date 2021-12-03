using System;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Scheduling;
using JetBrains.Annotations;

namespace Arbor.AppModel.Tests;

public class DelayScheduledService : TestScheduledService
{
    private readonly TimeSpan _delay;

    public DelayScheduledService(TimeSpan delay, ISchedule schedule, IScheduler scheduler) :
        base(schedule, scheduler) => _delay = delay;

    public override Task RunAsync(DateTimeOffset currentTime, CancellationToken stoppingToken) => Task.Delay(_delay, stoppingToken);
}