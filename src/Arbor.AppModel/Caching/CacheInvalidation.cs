using System;
using System.Threading.Tasks;
using Arbor.AppModel.ExtensionMethods;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace Arbor.AppModel.Caching
{
    public static class CacheInvalidation
    {
        private static readonly object MutexLock = new();

        public static async Task Invalidate(this IDistributedCache? distributedCache,
            CurrentCacheVersion? currentCacheVersion,
            InvalidateCache invalidateCache,
            ILogger logger)
        {
            if (distributedCache is { } && !string.IsNullOrWhiteSpace(invalidateCache.Prefix))
            {
                try
                {
                    await distributedCache.RemoveAsync(invalidateCache.Prefix);
                }
                catch (Exception ex) when (!ex.IsFatal())
                {
                    logger.Debug("Could not remove distributed cache with key {Key}", invalidateCache.Prefix);
                }
            }
            else if (currentCacheVersion is { })
            {
                lock (MutexLock)
                {
                    currentCacheVersion.CurrentVersion =
                        new CacheVersion(currentCacheVersion.CurrentVersion.Version + 1);
                }
            }
        }
    }
}