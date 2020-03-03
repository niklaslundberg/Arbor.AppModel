using System.Collections.Immutable;
using System.Reflection;
using System.Threading.Tasks;

namespace Arbor.App.Extensions.Application
{
    public interface IApplicationAssemblyResolver
    {
        Task<ImmutableArray<Assembly>> GetAssemblies(string[] assemblyNameStartsWith,bool cacheEnabled = true);
    }
}