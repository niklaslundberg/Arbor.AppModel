﻿using System;
using JetBrains.Annotations;

namespace Arbor.AppModel.ExtensionMethods
{
    public static class NullExceptionsExtensions
    {
        [NotNull]
        public static T ThrowIfNull<T>([CanBeNull] this T item) where T : class
        {
            if (item is null)
            {
                throw new ArgumentNullException($"Type {typeof(T).Name} is null");
            }

            return item;
        }

        [NotNull]
        public static string ThrowIfNullOrEmpty([CanBeNull] this string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                throw new ArgumentNullException(item, "Type string is null");
            }

            return item;
        }
    }
}