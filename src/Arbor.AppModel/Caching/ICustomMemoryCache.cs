using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Arbor.AppModel.Caching
{
    public interface ICustomMemoryCache //TODO extract from project
    {
        IReadOnlyCollection<string> CachedKeys { get; }

        bool TryGetValue<T>(string key, out T? item) where T : class;

        void SetValue<T>(string key, T item, TimeSpan? cacheTime = default) where T : class;

        void Invalidate(string? prefix = null);
    }
}