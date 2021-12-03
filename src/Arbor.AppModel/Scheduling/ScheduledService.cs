using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arbor.AppModel.Scheduling
{
    public abstract class ScheduledService
    {
        protected ScheduledService(ISchedule schedule) => Schedule = schedule;

        public ISchedule Schedule { get; }

        public virtual string Name => GetType().Name;

        public override string ToString() => Name;

        public virtual Task RunAsync(DateTimeOffset currentTime, CancellationToken stoppingToken) =>
            Task.CompletedTask;

        public SchedulingOptions SchedulingOptions { get; set; } = new();
    }
}