using Arbor.KVConfiguration.Core;

namespace Arbor.AppModel.Configuration
{
    public static class AppSettingsBuilderExtensions
    {
        public static AppSettingsBuilder Add(this AppSettingsBuilder builder, AppSettingsBuilder next) =>
            builder.Add(next.KeyValueConfiguration);
    }
}