using Arbor.AppModel.Application;
using Arbor.AppModel.DependencyInjection;
using Arbor.AppModel.ExtensionMethods;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel
{
    public class PreStartRegistrationModule : IModule
    {
        private readonly IApplicationAssemblyResolver _applicationAssemblyResolver;

        public PreStartRegistrationModule(IApplicationAssemblyResolver applicationAssemblyResolver) =>
            _applicationAssemblyResolver = applicationAssemblyResolver;

        public IServiceCollection Register(IServiceCollection builder)
        {
            var filteredAssemblies = _applicationAssemblyResolver.GetAssemblies();

            var loadablePublicConcreteTypesImplementing =
                filteredAssemblies.GetLoadablePublicConcreteTypesImplementing<IPreStartModule>();

            foreach (var loadable in loadablePublicConcreteTypesImplementing)
            {
                builder.AddSingleton(typeof(IPreStartModule), loadable, this);
            }

            return builder;
        }
    }
}