using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace Arbor.App.Extensions.ExtensionMethods
{
    [PublicAPI]
    public static class EnumerableExtensions
    {
        public static ImmutableArray<T> ThrowIfDefault<T>(this ImmutableArray<T> array)
        {
            if (array.IsDefault)
            {
                throw new InvalidOperationException($"The immutable array of {typeof(T).FullName} must not be default");
            }

            return array;
        }

        public static IReadOnlyCollection<T> SafeToReadOnlyCollection<T>(this IEnumerable<T>? items)
        {
            if (items == null)
            {
                return ImmutableArray<T>.Empty;
            }

            if (items is IList<T> list)
            {
                return new ReadOnlyCollection<T>(list);
            }

            if (items is IReadOnlyCollection<T> readOnly)
            {
                return readOnly;
            }

            return new ReadOnlyCollection<T>(new List<T>(items));
        }

        public static ImmutableArray<T> SafeToImmutableArray<T>(this IEnumerable<T>? items)
        {
            if (items == null)
            {
                return ImmutableArray<T>.Empty;
            }

            if (items is ImmutableArray<T> immutable)
            {
                return immutable;
            }

            return items.ToImmutableArray();
        }

        public static IReadOnlyCollection<T> AddDefaultValueIfEmpty<T>(this IReadOnlyCollection<T>? items)
            where T : struct
        {
            if (items is null)
            {
                return new List<T> {default};
            }

            if (items.Count == 0)
            {
                return new List<T> {default};
            }

            return items;
        }

        public static IReadOnlyCollection<T?> AddDefaultItemIfEmpty<T>(this IReadOnlyCollection<T>? items)
            where T : class
        {
            if (items is null)
            {
                return new List<T?> {default};
            }

            if (items.Count == 0)
            {
                return new List<T?> {default};
            }

            return items;
        }

        public static IEnumerable<T> Tap<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return TapInternal(enumerable, action);
        }

        public static IEnumerable<T> TapInternal<T>(this IEnumerable<T> enumerable, Action<T> action)
        {

            foreach (var item in enumerable)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> items) where T : class =>
            items
                .Where(item => item is { })
                .Select(item => item!);
    }
}