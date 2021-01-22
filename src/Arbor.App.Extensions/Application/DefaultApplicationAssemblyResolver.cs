using System.Collections.Immutable;
using System.Reflection;

namespace Arbor.App.Extensions.Application
{
    public class DefaultApplicationAssemblyResolver : IApplicationAssemblyResolver
    {
        private readonly ImmutableArray<Assembly> _assemblies;

        public DefaultApplicationAssemblyResolver(string[] assemblyNameStartsWith, bool cacheEnabled = true) =>
            _assemblies = ApplicationAssemblies.FilteredAssemblies(assemblyNameStartsWith, cacheEnabled);

        public ImmutableArray<Assembly>
            GetAssemblies() =>
            _assemblies;
    }
}