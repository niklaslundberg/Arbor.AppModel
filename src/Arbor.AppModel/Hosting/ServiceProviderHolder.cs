using System;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Hosting;

public class ServiceProviderHolder(
    IServiceProvider serviceProvider,
    IServiceCollection serviceCollection)
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public IServiceCollection ServiceCollection { get; } = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
}