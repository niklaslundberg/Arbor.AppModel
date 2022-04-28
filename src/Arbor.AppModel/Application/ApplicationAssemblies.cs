using System;
using System.Collections.Immutable;
using System.Reflection;

namespace Arbor.AppModel.Application
{
    public static class ApplicationAssemblies
    {
        public static ImmutableArray<Assembly> FilteredAssemblies(string[]? assemblyNameStartsWith = null,
            bool useCache = true) => AppDomain.CurrentDomain.FilteredAssemblies(assemblyNameStartsWith, useCache);
    }
}