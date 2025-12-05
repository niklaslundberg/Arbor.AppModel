using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Arbor.AppModel.ExtensionMethods;
using Arbor.AppModel.Logging;
using Microsoft.Extensions.DependencyModel;
using Serilog;
using Serilog.Core;

namespace Arbor.AppModel.Application
{
    public static class AppDomainExtensions
    {
        private static ImmutableArray<Assembly> _cache;

        private static void ForceLoadReferenceAssemblies()
        {
            Type[] types = [typeof(AppDomainExtensions)];

            foreach (var type in types)
            {
                TempLogger.WriteLine(type.Assembly.GetName().Name!);
            }
        }

        public static ImmutableArray<Assembly> FilteredAssemblies(this AppDomain appDomain,
            string[]? assemblyNameStartsWith = null,
            bool useCache = true,
            ILogger? logger = null)
        {
            logger ??= Logger.None;

            ArgumentNullException.ThrowIfNull(appDomain);

            if (useCache && !_cache.IsDefaultOrEmpty)
            {
                return _cache;
            }

            ForceLoadReferenceAssemblies();

            string? first = Assembly.GetEntryAssembly()?.FullName?.Split(".", StringSplitOptions.RemoveEmptyEntries)[0];

            var items = new List<string> { "Arbor" };

            if (!string.IsNullOrWhiteSpace(first))
            {
                items.Add(first);
            }

            string[] allowedAssemblies = assemblyNameStartsWith ?? items.ToArray();

            try
            {
                var defaultRuntimeLibraries = DependencyContext.Default?.RuntimeLibraries?.ToImmutableArray() ??
                                              ImmutableArray<RuntimeLibrary>.Empty;

                var includedLibraries = allowedAssemblies.Length == 0
                    ? defaultRuntimeLibraries
                    : [
                        ..defaultRuntimeLibraries.Where(item =>
                            allowedAssemblies.Any(
                                listed => item.Name.StartsWith(listed,
                                    StringComparison.OrdinalIgnoreCase)))
                    ];

                var loadedAssemblies = appDomain.GetAssemblies().Where(assembly =>
                                                     !assembly.IsDynamic &&
                                                     allowedAssemblies.Any(allowed =>
                                                         assembly.GetName().Name?.StartsWith(allowed,
                                                             StringComparison.OrdinalIgnoreCase) ??
                                                         false))
                                                .ToArray();

                var loadedAssemblyNames = loadedAssemblies.Select(assembly => assembly.GetName()).ToArray();

                var toLoad = includedLibraries.Where(lib =>
                    !loadedAssemblyNames.Any(loaded =>
                        loaded.Name?.Equals(lib.Name, StringComparison.Ordinal) ?? false)).ToArray();

                var assemblyNames = toLoad.SelectMany(lib => lib.GetDefaultAssemblyNames(DependencyContext.Default))
                                          .ToArray();

                foreach (var assemblyName in assemblyNames)
                {
                    if (!appDomain.GetAssemblies().Any(assembly =>
                        !assembly.IsDynamic && assembly.GetName().FullName == assemblyName.FullName))
                    {
                        appDomain.Load(assemblyName);
                    }
                }
            }
            catch (Exception ex) when (!ex.IsFatal())
            {
                logger.Warning(ex, "Could not load runtime assemblies");
            }

            var orders = new List<(string, int)> { ("test", 1000), ("debug", 2000) };

            int GetAssemblyLoadOrder(Assembly assembly)
            {
                string? assemblyName = assembly.GetName().Name;

                const int defaultOrder = 0;

                if (string.IsNullOrWhiteSpace(assemblyName))
                {
                    return defaultOrder;
                }

                foreach ((string name, int order) in orders!)
                {
                    if (assemblyName.Contains(name, StringComparison.OrdinalIgnoreCase))
                    {
                        return order;
                    }
                }

                return defaultOrder;
            }

            var filteredAssemblies = appDomain.GetAssemblies().Where(assembly =>
                !assembly.IsDynamic &&
                allowedAssemblies.Any(listed =>
                    assembly.FullName?.StartsWith(listed, StringComparison.OrdinalIgnoreCase) == true)).Select(
                assembly =>
                {
                    (Assembly Assembly, int Order) tuple = (assembly, GetAssemblyLoadOrder(assembly));

                    return tuple;
                }).OrderBy(tuple => tuple.Order).Select(tuple => tuple.Assembly).ToImmutableArray();

            if (useCache)
            {
                _cache = filteredAssemblies;
            }

            return filteredAssemblies;
        }
    }
}