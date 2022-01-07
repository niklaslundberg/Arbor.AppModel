using System;
using Arbor.AppModel.Scheduling;

namespace Arbor.AppModel.Sample;

public class ScheduleEvery : ISchedule
{
    private DateTimeOffset _next;
    public ScheduleEvery()
    {

    }

    public DateTimeOffset? Next(DateTimeOffset currentTime)
    {
        if (_next == DateTimeOffset.MinValue)
        {
            _next = currentTime.AddSeconds(2);
        }
        else if (currentTime > _next)
        {
            _next = _next.AddSeconds(5);
        }

        return _next;
    }
}