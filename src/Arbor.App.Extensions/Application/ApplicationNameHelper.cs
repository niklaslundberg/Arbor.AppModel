using System.Reflection;
using Arbor.KVConfiguration.Core;

namespace Arbor.App.Extensions.Application
{
    public static class ApplicationNameHelper
    {
        public static string? GetApplicationName(this IKeyValueConfiguration keyValueConfiguration)
        {
            string name = keyValueConfiguration[ApplicationConstants.ApplicationNameKey];

            if (string.IsNullOrWhiteSpace(name))
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }

            return name;
        }
    }
}