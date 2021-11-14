using Arbor.AppModel.Application;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace Arbor.AppModel.Hosting
{
    public static class EnvironmentConfigurationExtensions
    {
        public static IHostEnvironment ToHostEnvironment(this EnvironmentConfiguration environmentConfiguration) =>
            new HostingEnvironment { EnvironmentName = environmentConfiguration.EnvironmentName };
    }
}