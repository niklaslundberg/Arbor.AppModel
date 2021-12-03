using System;
using System.Threading;
using System.Threading.Tasks;

namespace Arbor.AppModel.Scheduling
{
    public interface ITimer : IDisposable
    {
        void Register(IScheduler scheduler);

        Task Run(CancellationToken stoppingToken);
    }
}