using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Arbor.AppModel.Scheduling
{
    public interface IScheduler
    {
        public ImmutableArray<ScheduledService> Schedules { get; }

        public bool Add(ScheduledService schedule, OnTickAsync onTick);

        public Task Tick(CancellationToken stoppingToken);
    }
}