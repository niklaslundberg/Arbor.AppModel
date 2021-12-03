using System;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Scheduling;
using JetBrains.Annotations;

namespace Arbor.AppModel.Tests
{
    public class TestScheduledService : ScheduledService
    {
        public TestScheduledService(ISchedule schedule, IScheduler scheduler) : base(schedule) => scheduler.Add(this, RunAsync);

        public int Invokations { get; protected set; }
        public int CompletedInvokations { get; protected set; }

        public override Task RunAsync(DateTimeOffset currentTime, CancellationToken stoppingToken)
        {
            ++Invokations;
            ++CompletedInvokations;

            return Task.CompletedTask;
        }
    }
}