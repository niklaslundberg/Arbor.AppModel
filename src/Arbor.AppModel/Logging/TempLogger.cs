using System;
using System.Collections.Concurrent;
using System.Threading;
using Serilog;

namespace Arbor.AppModel.Logging
{
    public static class TempLogger
    {
        private static readonly ConcurrentQueue<string> LogMessages = new();

        public static void WriteLine(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                LogMessages.Enqueue(message);
            }
        }

        public static void FlushWith(ILogger logger)
        {
            while (LogMessages.TryDequeue(out string? message))
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    logger.Information("{Message}", message);
                }
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }
    }
}