using System;

namespace Arbor.AppModel.Scheduling;

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