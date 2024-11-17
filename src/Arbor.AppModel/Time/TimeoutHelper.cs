using System;
using System.Threading;

namespace Arbor.AppModel.Time;

public class TimeoutHelper(TimeoutConfiguration? timeoutConfiguration = null)
{
    public CancellationTokenSource CreateCancellationTokenSource(TimeSpan? timeSpan = default)
    {
        if (timeoutConfiguration?.CancellationEnabled == false)
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