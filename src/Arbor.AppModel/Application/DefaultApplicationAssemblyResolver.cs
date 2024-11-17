using System.Collections.Immutable;
using System.Reflection;

namespace Arbor.AppModel.Application;

public class DefaultApplicationAssemblyResolver(string[] assemblyNameStartsWith, bool cacheEnabled = true)
    : IApplicationAssemblyResolver
{
    private readonly ImmutableArray<Assembly> _assemblies = ApplicationAssemblies.FilteredAssemblies(assemblyNameStartsWith, cacheEnabled);

    public ImmutableArray<Assembly> GetAssemblies() => _assemblies;
}