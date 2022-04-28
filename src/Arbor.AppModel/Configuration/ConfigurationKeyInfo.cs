using System;
using System.Linq;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;

namespace Arbor.AppModel.Configuration
{
    public class ConfigurationKeyInfo
    {
        public ConfigurationKeyInfo([NotNull] string key, string? value, string? source = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            Key = key;
            Value = value.MakeAnonymous(key, ArborStringExtensions.DefaultAnonymousKeyWords.ToArray());
            Source = source;
        }

        public string Key { get; }

        public string? Value { get; }

        public string? Source { get; }

        public override string ToString() =>
            $"{nameof(Key)}: {Key}, {nameof(Value)}: '{Value}', {nameof(Source)}: {Source}";
    }
}