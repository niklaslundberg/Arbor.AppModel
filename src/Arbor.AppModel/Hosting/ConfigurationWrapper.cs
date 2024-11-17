using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Arbor.AppModel.Hosting;

public class ConfigurationWrapper(
    IConfigurationRoot hostingContextConfiguration,
    ServiceProviderHolder serviceProviderHolder)
    : IConfigurationRoot
{
    public ServiceProviderHolder ServiceProviderHolder { get; } = serviceProviderHolder;

    public IEnumerable<IConfigurationProvider> Providers => hostingContextConfiguration.Providers;

    public string? this[string key]
    {
        get => hostingContextConfiguration[key];
        set => hostingContextConfiguration[key] = value;
    }

    public IEnumerable<IConfigurationSection> GetChildren() => hostingContextConfiguration.GetChildren();

    public IChangeToken GetReloadToken() => hostingContextConfiguration.GetReloadToken();

    public IConfigurationSection GetSection(string key) => hostingContextConfiguration.GetSection(key);

    public void Reload() => hostingContextConfiguration.Reload();
}