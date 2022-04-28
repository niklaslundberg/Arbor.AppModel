using Arbor.AppModel.Application;
using Arbor.AppModel.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Messaging
{
    [UsedImplicitly]
    public class MediatorModule : IModule
    {
        private readonly IApplicationAssemblyResolver _applicationAssemblyResolver;

        public MediatorModule(IApplicationAssemblyResolver applicationAssemblyResolver) =>
            _applicationAssemblyResolver = applicationAssemblyResolver;

        public IServiceCollection Register(IServiceCollection builder) =>
            MediatorRegistrationHelper.Register(builder, _applicationAssemblyResolver.GetAssemblies(), this);
    }
}