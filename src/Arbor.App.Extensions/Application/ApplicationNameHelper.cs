using System;
using System.Reflection;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.App.Extensions.Application
{
    public static class ApplicationNameHelper
    {
        public static string? GetApplicationName([NotNull] this IKeyValueConfiguration keyValueConfiguration)
        {
            if (keyValueConfiguration == null)
            {
                throw new ArgumentNullException(nameof(keyValueConfiguration));
            }

            string name = keyValueConfiguration[ApplicationConstants.ApplicationNameKey];

            if (string.IsNullOrWhiteSpace(name))
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }

            return name;
        }
    }
}