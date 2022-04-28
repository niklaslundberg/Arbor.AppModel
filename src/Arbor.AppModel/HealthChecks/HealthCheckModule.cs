using System;
using Arbor.AppModel.Application;
using Arbor.AppModel.DependencyInjection;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.HealthChecks
{
    [UsedImplicitly]
    public class HealthCheckModule : IModule
    {
        public IServiceCollection Register(IServiceCollection builder)
        {
            foreach (Type type in ApplicationAssemblies.FilteredAssemblies()
                                                       .GetLoadablePublicConcreteTypesImplementing<IHealthCheck>())
            {
                builder.AddSingleton(type, this);
            }

            return builder.AddSingleton<HealthChecker>(this);
        }
    }
}