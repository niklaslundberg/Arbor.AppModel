using Arbor.AppModel.Application;
using Arbor.AppModel.DependencyInjection;
using Arbor.AppModel.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel;

public class PreStartRegistrationModule(IApplicationAssemblyResolver applicationAssemblyResolver) : IModule
{
    public IServiceCollection Register(IServiceCollection builder)
    {
        var filteredAssemblies = applicationAssemblyResolver.GetAssemblies();

        var loadablePublicConcreteTypesImplementing =
            filteredAssemblies.GetLoadablePublicConcreteTypesImplementing<IPreStartModule>();

        foreach (var loadable in loadablePublicConcreteTypesImplementing)
        {
            builder.AddSingleton(typeof(IPreStartModule), loadable, this);
        }

        return builder;
    }
}