using System;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Hosting
{
    public class ServiceProviderHolder
    {
        public ServiceProviderHolder(IServiceProvider serviceProvider,
            IServiceCollection serviceCollection)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            ServiceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
        }

        public IServiceProvider ServiceProvider { get; }

        public IServiceCollection ServiceCollection { get; }
    }
}