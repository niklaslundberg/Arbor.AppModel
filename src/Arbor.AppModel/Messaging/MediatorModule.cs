using Arbor.AppModel.Application;
using Arbor.AppModel.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Messaging;

[UsedImplicitly]
public class MediatorModule(IApplicationAssemblyResolver applicationAssemblyResolver) : IModule
{
    public IServiceCollection Register(IServiceCollection builder) =>
        MediatorRegistrationHelper.Register(builder, applicationAssemblyResolver.GetAssemblies(), this);
}