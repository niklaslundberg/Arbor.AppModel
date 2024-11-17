using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Arbor.AppModel.Caching;

[UsedImplicitly]
public class CustomMemoryCache(IMemoryCache memoryCache, ILogger logger) : ICustomMemoryCache
{
    private static readonly ConcurrentDictionary<string, object> Keys = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyCollection<string> CachedKeys => GetCachedKeys();

    public bool TryGetValue<T>(string key, out T? item) where T : class
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        }

        if (memoryCache.TryGetValue(key, out object? cachedItem) && cachedItem is T cachedItemOfT)
        {
            item = cachedItemOfT;
            return true;
        }

        item = default;
        return false;
    }

    public void SetValue<T>(string key, T item, TimeSpan? cacheTime = default) where T : class
    {
        ArgumentNullException.ThrowIfNull(item);

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
        }

        var cacheEntryAbsoluteExpirationRelativeToNow =
            cacheTime?.TotalSeconds > 0 ? cacheTime.Value : TimeSpan.FromSeconds(900);

        memoryCache.Set(key, item, cacheEntryAbsoluteExpirationRelativeToNow);
        bool added = Keys.TryAdd(key, string.Empty);

        if (!added && !Keys.ContainsKey(key))
        {
            logger.Debug("Could not add item with key {Key} to cache", key);
        }
    }

    public void Invalidate(string? prefix = null)
    {
        IReadOnlyCollection<string> keys = GetCachedKeys();

        if (keys.Count == 0)
        {
            logger.Debug("No items were removed from in-memory cache since there were no cached items");
            return;
        }

        IReadOnlyCollection<string> filteredKeys = keys;

        if (prefix.HasSomeString())
        {
            filteredKeys = keys.Where(key => key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        logger.Debug(
            "Removing {ToRemoveCount} items of {TotalCount} from in-memory cache matching prefix {Prefix}",
            filteredKeys.Count,
            keys.Count,
            prefix);

        foreach (string key in filteredKeys)
        {
            memoryCache.Remove(key);
            Keys.TryRemove(key, out _);
            logger.Debug("Removed item with key {CacheKey} from in-memory cache", key);
        }
    }

    private IReadOnlyCollection<string> GetCachedKeys()
    {
        (string key, bool exists)[] keys = Keys.Select(key => (key.Key, memoryCache.TryGetValue(key.Key, out _)))
            .ToArray();

        string[] toRemove = keys.Where(item => !item.exists).Select(item => item.key).ToArray();

        foreach (string nonCachedKey in toRemove)
        {
            bool removed = Keys.TryRemove(nonCachedKey, out _);

            if (!removed)
            {
                logger.Debug("Could not remove cached item with key {CacheKey}", nonCachedKey);
            }
        }

        return Keys.Keys.ToArray();
    }
}