using System;
using System.Collections.Generic;
using System.Linq;
using Arbor.AppModel.Application;
using Arbor.AppModel.DependencyInjection;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Startup;

[UsedImplicitly]
public class StartupTaskModule(IApplicationAssemblyResolver assemblyResolver) : IModule
{
    public IServiceCollection Register(IServiceCollection builder)
    {
        IEnumerable<Type> startupTaskTypes = assemblyResolver.GetAssemblies()
            .SelectMany(assembly => assembly.GetLoadableTypes())
            .Where(t => t
                .IsPublicConcreteTypeImplementing<
                    IStartupTask>());

        foreach (Type startupTask in startupTaskTypes)
        {
            builder.AddSingleton<IStartupTask>(context => context.GetRequiredService(startupTask), this);

            if (builder.Any(serviceDescriptor => serviceDescriptor.ImplementationType == startupTask &&
                                                 serviceDescriptor.ServiceType == startupTask))
            {
                continue;
            }

            builder.AddSingleton(startupTask, this);
        }

        return builder;
    }
}