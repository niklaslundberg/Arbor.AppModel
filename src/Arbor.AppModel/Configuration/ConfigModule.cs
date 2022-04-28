using Arbor.AppModel.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Configuration
{
    public class ConfigModule : IModule
    {
        public IServiceCollection Register(IServiceCollection builder) => builder.AddSingleton<UserConfigUpdater>();
    }
}