using System;

namespace Arbor.AppModel.Scheduling;

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