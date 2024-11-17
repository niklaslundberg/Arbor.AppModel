using System.Collections.Immutable;
using System.Reflection;

namespace Arbor.AppModel.Application;

public class InstanceApplicationAssemblyResolver(ImmutableArray<Assembly> assemblies) : IApplicationAssemblyResolver
{
    public ImmutableArray<Assembly> GetAssemblies() => assemblies;
}