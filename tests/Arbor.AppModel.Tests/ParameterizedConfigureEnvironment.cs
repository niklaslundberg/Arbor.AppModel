using Arbor.AppModel.DependencyInjection;
using Arbor.Primitives;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Arbor.AppModel.Tests
{
    [UsedImplicitly]
    public class ParameterizedConfigureEnvironment : IModule
    {
        public ParameterizedConfigureEnvironment(EnvironmentVariables environmentVariables)
        {
        }

        public IServiceCollection Register(IServiceCollection builder) => builder;
    }
}