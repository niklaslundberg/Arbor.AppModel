using System;

namespace Arbor.AppModel.Scheduling
{
    public interface ISchedule
    {
        public DateTimeOffset? Next(DateTimeOffset currentTime);
    }
}