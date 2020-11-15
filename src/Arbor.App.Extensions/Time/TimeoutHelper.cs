using System;
using System.Threading;

namespace Arbor.App.Extensions.Time
{
    public class TimeoutHelper
    {
        private readonly TimeoutConfiguration? _timeoutConfiguration;

        public TimeoutHelper(TimeoutConfiguration? timeoutConfiguration = null) =>
            _timeoutConfiguration = timeoutConfiguration;

        public CancellationTokenSource CreateCancellationTokenSource(TimeSpan? timeSpan = default)
        {
            if (_timeoutConfiguration?.CancellationEnabled == false)
            {
                return new CancellationTokenSource();
            }

            if (timeSpan is null)
            {
                return new CancellationTokenSource();
            }

            return new CancellationTokenSource(timeSpan.Value);
        }
    }
}