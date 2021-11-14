using System;
using System.Linq;
using Arbor.AppModel.Configuration;
using Arbor.KVConfiguration.Urns;

namespace Arbor.AppModel.Application
{
    public static class EnvironmentConfigurator
    {
        public static void ConfigureEnvironment(ConfigurationInstanceHolder configurationInstanceHolder)
        {
            if (configurationInstanceHolder is null)
            {
                throw new ArgumentNullException(nameof(configurationInstanceHolder));
            }

            var configureEnvironments = configurationInstanceHolder.CreateInstances<IConfigureEnvironment>();
            var environmentConfiguration = configurationInstanceHolder.Get<EnvironmentConfiguration>();

            if (environmentConfiguration is null)
            {
                var newConfiguration = new EnvironmentConfiguration();
                environmentConfiguration = newConfiguration;

                configurationInstanceHolder.Add(
                    new NamedInstance<EnvironmentConfiguration>(newConfiguration, "default"));
            }

            var ordered = configureEnvironments
                         .Select(environmentConfigurator => (EnvironmentConfigurator: environmentConfigurator,
                              Order: environmentConfigurator.GetRegistrationOrder(0))).OrderBy(pair => pair.Order)
                         .Select(pair => pair.EnvironmentConfigurator).ToArray();

            foreach (var configureEnvironment in ordered)
            {
                configureEnvironment.Configure(environmentConfiguration);
            }
        }
    }
}