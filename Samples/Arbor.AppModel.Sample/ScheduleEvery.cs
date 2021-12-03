using System;
using Arbor.AppModel.Scheduling;

namespace Arbor.AppModel.Sample;

public class ScheduleEvery : ISchedule
{
    public DateTimeOffset? Next(DateTimeOffset currentTime)
    {
        return currentTime.AddSeconds(5);
    }
}