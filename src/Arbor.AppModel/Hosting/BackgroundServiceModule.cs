using System.Linq;
using Arbor.AppModel.Application;
using Arbor.AppModel.DependencyInjection;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Arbor.AppModel.Hosting;

[UsedImplicitly]
public class BackgroundServiceModule(IApplicationAssemblyResolver applicationAssemblyResolver) : IModule
{
    public IServiceCollection Register(IServiceCollection builder)
    {
        var types = applicationAssemblyResolver.GetAssemblies()
            .GetLoadablePublicConcreteTypesImplementing<IHostedService>();

        foreach (var type in types)
        {
            builder.AddSingleton<IHostedService>(context => context.GetRequiredService(type), this);

            if (builder.Any(serviceDescriptor =>
                    serviceDescriptor.ImplementationType == type && serviceDescriptor.ServiceType == type))
            {
                continue;
            }

            builder.AddSingleton(type, this);
        }

        return builder;
    }
}