using System.Collections.Immutable;
using System.Reflection;

namespace Arbor.AppModel.Application
{
    public interface IApplicationAssemblyResolver
    {
        ImmutableArray<Assembly> GetAssemblies();
    }
}