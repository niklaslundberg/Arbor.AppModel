using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;

namespace Arbor.App.Extensions.Application
{
    public class DefaultApplicationAssemblyResolver : IApplicationAssemblyResolver
    {
        public Task<ImmutableArray<Assembly>>
            GetAssemblies(string[] assemblyNameStartsWith, bool cacheEnabled = true) =>
            Task.FromResult(ApplicationAssemblies.FilteredAssemblies(assemblyNameStartsWith, cacheEnabled));
    }
}