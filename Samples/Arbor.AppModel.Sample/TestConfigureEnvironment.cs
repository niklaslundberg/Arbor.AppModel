using Arbor.AppModel.Application;
using Arbor.AppModel.Configuration;

namespace Arbor.AppModel.Sample
{
    public class TestConfigureEnvironment : IConfigureEnvironment
    {
        public const int HttpPort = 15004;

        public void Configure(EnvironmentConfiguration environmentConfiguration)
        {
            environmentConfiguration.HttpEnabled = true;
            environmentConfiguration.HttpPort = HttpPort;
        }
    }
}