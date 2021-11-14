using System;

namespace Arbor.AppModel.Scheduling
{
    public interface ITimer : IDisposable
    {
        void Register(Action onTick);
    }
}