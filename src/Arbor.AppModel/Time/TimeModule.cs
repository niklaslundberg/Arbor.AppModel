using Arbor.AppModel.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Time
{
    [UsedImplicitly]
    public class TimeModule : IModule
    {
        public IServiceCollection Register(IServiceCollection builder) => builder
                                                                         .AddSingleton<ICustomClock, CustomSystemClock>(
                                                                              this).AddSingleton<TimeoutHelper>(this);
    }
}