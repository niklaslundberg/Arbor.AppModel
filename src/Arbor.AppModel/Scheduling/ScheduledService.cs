using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arbor.AppModel.Scheduling
{
    public abstract class ScheduledService
    {
        protected ScheduledService(ISchedule schedule)
        {
            Schedule = schedule;
        }

        public ISchedule Schedule { get; }

        public virtual string Name => GetType().Name;

        public override string ToString() => Name;

        public virtual Task RunAsync(DateTimeOffset currentTime, CancellationToken stoppingToken) =>
            Task.CompletedTask;

        public SchedulingOptions SchedulingOptions { get; set; } = new();
    }

    public class SchedulingOptions
    {
        public SchedulingOptions(SchedulingDelta? schedulingDelta = default, SchedulingBehavior? schedulingBehavior = default)
        {
            SchedulingBehavior = schedulingBehavior ?? SchedulingBehavior.Default;
            SchedulingDelta = schedulingDelta ?? new SchedulingDelta { Diff = TimeSpan.FromMilliseconds(20) };
        }

        public SchedulingBehavior SchedulingBehavior { get; }
        public SchedulingDelta SchedulingDelta { get; }
    }

    public class SchedulingDelta
    {
        public TimeSpan Diff { get; set; }
    }

    public sealed class SchedulingBehavior : IEquatable<SchedulingBehavior>
    {
        public bool Equals(SchedulingBehavior? other) => other is {} && (ReferenceEquals(this, other) || Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase));

        public override bool Equals(object? obj) => obj is SchedulingBehavior other && Equals(other);

        public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);

        public static bool operator ==(SchedulingBehavior? left, SchedulingBehavior? right) => Equals(left, right);

        public static bool operator !=(SchedulingBehavior? left, SchedulingBehavior? right) => !Equals(left, right);

        public string Name { get; }

        public static readonly SchedulingBehavior Skip = new(nameof(Skip));
        public static readonly SchedulingBehavior StopExisting = new(nameof(StopExisting));
        public static readonly SchedulingBehavior RescheduleAfter = new(nameof(RescheduleAfter));
        public static readonly SchedulingBehavior RunInParallel = new(nameof(RunInParallel));

        public static SchedulingBehavior Default => Skip;

        private SchedulingBehavior(string name) => Name = name;
    }
}