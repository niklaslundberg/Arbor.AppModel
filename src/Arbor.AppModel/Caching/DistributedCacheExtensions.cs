using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Serilog;

namespace Arbor.AppModel.Caching
{
    public static class DistributedCacheExtensions
    {
        public static Task SetWithVersionAsync<T>(this IDistributedCache cache,
            string key,
            T item,
            CacheVersion cacheVersion,
            DistributedCacheEntryOptions? options = default,
            ILogger? logger = default,
            CancellationToken cancellationToken = default) where T : class
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return SetWithVersionInternalAsync(cache, key, item, cacheVersion, options, logger, cancellationToken);
        }

        private static async Task SetWithVersionInternalAsync<T>(IDistributedCache cache,
            string key,
            T item,
            CacheVersion cacheVersion,
            DistributedCacheEntryOptions? options = default,
            ILogger? logger = default,
            CancellationToken cancellationToken = default) where T : class
        {
            string json =
                JsonConvert.SerializeObject(new VersionedJson<T> { Instance = item, Version = cacheVersion.Version });

            try
            {
                if (options is { })
                {
                    await cache.SetStringAsync(key, json, options, cancellationToken);
                    return;
                }

                await cache.SetStringAsync(key, json, cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                logger?.Debug(ex, "Cache timed out");
            }
            catch (OperationCanceledException ex)
            {
                logger?.Debug(ex, "Cache timed out");
            }
            catch (Exception ex)
            {
                logger?.Debug(ex, "Could not set cache");
            }
        }

        public static Task<T?> GetWithVersionAsync<T>(this IDistributedCache cache,
            string key,
            CacheVersion cacheVersion,
            ILogger? logger = default,
            CancellationToken cancellationToken = default) where T : class
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return GetWithVersionInternalAsync<T>(cache, key, cacheVersion, logger, cancellationToken);
        }

        private static async Task<T?> GetWithVersionInternalAsync<T>(IDistributedCache cache,
            string key,
            CacheVersion cacheVersion,
            ILogger? logger = default,
            CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                string? json = await cache.GetStringAsync(key, cancellationToken);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                var versionedObject = JsonConvert.DeserializeObject<VersionedJson<T>>(json);

                if (versionedObject?.Version == cacheVersion.Version)
                {
                    return versionedObject.Instance;
                }

                await cache.RemoveAsync(key, cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                logger?.Debug(ex, "Distributed cache timed out for key {Key}", key);
                return default;
            }
            catch (OperationCanceledException ex)
            {
                logger?.Debug(ex, "Distributed cache timed out for key {Key}", key);
                return default;
            }
            catch (Exception ex)
            {
                logger?.Debug(ex, "Could not get distributed cache item for key {Key}", key);
                return default;
            }

            return null;
        }

        private class VersionedJson<T>
        {
            public int Version { get; init; }

            public T? Instance { get; init; }
        }
    }
}