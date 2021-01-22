using System.Collections.Immutable;
using System.Reflection;

namespace Arbor.App.Extensions.Application
{
    public interface IApplicationAssemblyResolver
    {
        ImmutableArray<Assembly> GetAssemblies();
    }
}