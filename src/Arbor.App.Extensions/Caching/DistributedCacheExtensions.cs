using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Arbor.App.Extensions.Caching
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetWithVersionAsync<T>(this IDistributedCache cache, string key, [NotNull] T item,
            CacheVersion cacheVersion, CancellationToken cancellationToken = default) where T : class
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

            string? json =
                JsonConvert.SerializeObject(new VersionedJson<T> {Instance = item, Version = cacheVersion.Version});

            await cache.SetStringAsync(key, json, cancellationToken);
        }

        public static async Task<T?> GetWithVersionAsync<T>(this IDistributedCache cache, string key,
            CacheVersion cacheVersion, CancellationToken cancellationToken = default) where T : class
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                string? json = await cache.GetStringAsync(key, cancellationToken);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return null;
                }

                var versionedObject = JsonConvert.DeserializeObject<VersionedJson<T>>(json);

                if (versionedObject.Version == cacheVersion.Version)
                {
                    return versionedObject.Instance;
                }

                await cache.RemoveAsync(key, cancellationToken);
            }
            catch (Exception)
            {
                // ignore serialization issues
                return null;
            }

            return null;
        }

        private class VersionedJson<T>
        {
            public int Version { get; set; }
            public T? Instance { get; set; }
        }
    }
}