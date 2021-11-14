using System;
using System.Collections.Generic;
using Arbor.AppModel.Scheduling;

namespace Arbor.AppModel.Tests
{
    public sealed class TestTimer : ITimer
    {
        private readonly List<Action> _actions = new();

        public void Dispose()
        {
            // ignore
        }

        public void Register(Action onTick) => _actions.Add(onTick);

        public void Tick()
        {
            foreach (var action in _actions)
            {
                action();
            }
        }
    }
}