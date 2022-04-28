using System.Collections.Immutable;
using System.Reflection;

namespace Arbor.AppModel.Application
{
    public class InstanceApplicationAssemblyResolver : IApplicationAssemblyResolver
    {
        private readonly ImmutableArray<Assembly> _assemblies;

        public InstanceApplicationAssemblyResolver(ImmutableArray<Assembly> assemblies) => _assemblies = assemblies;

        public ImmutableArray<Assembly> GetAssemblies() => _assemblies;
    }
}