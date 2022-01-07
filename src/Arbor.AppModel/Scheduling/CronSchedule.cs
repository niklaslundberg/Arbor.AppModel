using System;
using Cronos;

namespace Arbor.AppModel.Scheduling
{
    public class CronSchedule : ISchedule
    {
        private readonly CronExpression _cronExpression;

        private DateTimeOffset? _next;

        public CronSchedule(CronExpression cronExpression) => _cronExpression = cronExpression;

        public DateTimeOffset? Next(DateTimeOffset currentTime)
        {
            var adjusted = currentTime;

            if (currentTime.Millisecond == 0)
            {
                adjusted = currentTime.UtcDateTime.AddMilliseconds(-1);
            }

            if (currentTime > _next || _next is null)
            {
               _next = _cronExpression.GetNextOccurrence(adjusted.UtcDateTime);
            }

            return _next;
        }
    }
}