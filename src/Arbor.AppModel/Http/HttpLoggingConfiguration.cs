using Arbor.AppModel.Configuration;
using Arbor.KVConfiguration.Core.Metadata;
using Arbor.KVConfiguration.Urns;
using JetBrains.Annotations;

namespace Arbor.AppModel.Http;

[UsedImplicitly]
[Urn(HttpLoggingConfigurationUrn)]
public class HttpLoggingConfiguration(bool enabled) : IConfigurationValues
{
    [Metadata(defaultValue: "true")]
    public const string HttpLoggingEnabled = "urn:arbor:app:http:http-logging:default:enabled";

    [Metadata]
    public const string HttpLoggingConfigurationUrn = "urn:arbor:app:http:http-logging";

    public bool Enabled { get; } = enabled;
}