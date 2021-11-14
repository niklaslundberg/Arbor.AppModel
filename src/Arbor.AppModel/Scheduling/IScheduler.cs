using System.Collections.Immutable;

namespace Arbor.AppModel.Scheduling
{
    public interface IScheduler
    {
        public ImmutableArray<ISchedule> Schedules { get; }

        public bool Add(ISchedule schedule, OnTickAsync onTick);
    }
}