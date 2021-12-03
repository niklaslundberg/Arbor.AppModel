using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Scheduling;

namespace Arbor.AppModel.Tests
{
    public sealed class TestTimer : ITimer
    {
        private readonly List<IScheduler> _actions = new();

        public void Dispose() => _actions.Clear();

        public void Register(IScheduler onTick) => _actions.Add(onTick);

        public async Task Tick(CancellationToken cancellationToken = default)
        {
            if (_actions.Count == 0)
            {
                return;
            }

            foreach (var action in _actions.ToArray())
            {
                try
                {
                    await action.Tick(cancellationToken);
                }
                catch (Exception)
                {
                    // ignore exception
                }
            }
        }

        public async Task Run(CancellationToken stoppingToken) {}
    }
}