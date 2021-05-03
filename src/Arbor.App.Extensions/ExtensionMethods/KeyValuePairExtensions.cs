﻿using System.Collections.Generic;

namespace Arbor.App.Extensions.ExtensionMethods
{
    public static class KeyValuePairExtensions
    {
        public static KeyValuePair<string, string> MakeAnonymousNullValue(this KeyValuePair<string, string?> pair) =>
            new(pair.Key, new string('*', 5));

        public static KeyValuePair<string, string> MakeAnonymousValue(this KeyValuePair<string, string> pair) =>
            new(pair.Key, new string('*', 5));

        public static string? ValueOrDefault(this IReadOnlyDictionary<string, string>? dictionary, string? key)
        {
            if (dictionary is null)
            {
                return default;
            }

            if (key is null)
            {
                return default;
            }

            bool found = dictionary.TryGetValue(key, out string? value);

            if (!found)
            {
                return default;
            }

            return value;
        }
    }
}