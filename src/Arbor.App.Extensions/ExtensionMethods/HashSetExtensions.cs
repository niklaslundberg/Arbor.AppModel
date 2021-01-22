using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Arbor.App.Extensions.ExtensionMethods
{
    public static class HashSetExtensions
    {
        public static void AddRange<T>([NotNull] this HashSet<T> hashSet, [NotNull] IEnumerable<T> items)
        {
            if (hashSet == null)
            {
                throw new ArgumentNullException(nameof(hashSet));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                hashSet.Add(item);
            }
        }
    }
}