using System;
using Arbor.AppModel.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Time
{
    [UsedImplicitly]
    public class TimeModule : IModule
    {
        public IServiceCollection Register(IServiceCollection builder) => builder
                                                                         .AddSingleton<TimeProvider>(TimeProvider.System).AddSingleton<TimeoutHelper>(this);
    }
}