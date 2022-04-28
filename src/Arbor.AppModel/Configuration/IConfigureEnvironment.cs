using Arbor.AppModel.Application;

namespace Arbor.AppModel.Configuration
{
    public interface IConfigureEnvironment
    {
        void Configure(EnvironmentConfiguration environmentConfiguration);
    }
}