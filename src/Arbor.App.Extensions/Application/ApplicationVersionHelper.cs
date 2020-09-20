using System;
using System.Diagnostics;
using System.Reflection;
using Arbor.App.Extensions.ExtensionMethods;

namespace Arbor.App.Extensions.Application
{
    public static class ApplicationVersionHelper
    {
        public static ApplicationVersionInfo? GetAppVersion()
        {
            var executingAssembly = typeof(ApplicationVersionHelper).Assembly;

            var assemblyName = executingAssembly.GetName();

            var assemblyVersion = assemblyName.Version;

            string? executingAssemblyFullName = executingAssembly.FullName;

            if (string.IsNullOrWhiteSpace(executingAssemblyFullName))
            {
                throw new InvalidOperationException("Could not get executing assembly full name");
            }

            if (assemblyVersion is null)
            {
                throw new InvalidOperationException(
                    $"Assembly version is null for assembly {executingAssemblyFullName}");
            }

            string assemblyVersionString = assemblyVersion.ToString().ThrowIfNullOrWhiteSpace();

            var assemblyInformationalVersionAttribute =
                executingAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            string location = executingAssembly.Location.ThrowIfNullOrWhiteSpace();

            var fvi = FileVersionInfo.GetVersionInfo(location);

            string? fileVersion = fvi.FileVersion;

            if (string.IsNullOrWhiteSpace(fileVersion))
            {
                return null;
            }

            return new ApplicationVersionInfo(assemblyVersionString,
                fileVersion,
                assemblyInformationalVersionAttribute?.InformationalVersion ?? fileVersion,
                executingAssemblyFullName);
        }
    }
}