﻿using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using Arbor.AppModel.Configuration;
using Arbor.AppModel.ExtensionMethods;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.AppModel.Application;

[RegistrationOrder(0)]
[UsedImplicitly]
public class ApplicationEnvironmentConfigurator(IKeyValueConfiguration keyValueConfiguration)
    : IConfigureEnvironment
{
    private readonly IKeyValueConfiguration _keyValueConfiguration = keyValueConfiguration ?? throw new ArgumentNullException(nameof(keyValueConfiguration));

    public void Configure(EnvironmentConfiguration environmentConfiguration)
    {
        ArgumentNullException.ThrowIfNull(environmentConfiguration);

        string proxiesValue = _keyValueConfiguration[ApplicationConstants.ProxyAddresses].WithDefault("")!;

        var proxies = proxiesValue.Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(ipString => (HasIp: IPAddress.TryParse(ipString, out var address),
                IpAddress: address)).Where(address => address.HasIp)
            .Select(address => address.IpAddress).ToImmutableArray();

        environmentConfiguration.ProxyAddresses.AddRange(proxies);

        environmentConfiguration.PublicHostname = _keyValueConfiguration[ApplicationConstants.PublicHostName];

        if (int.TryParse(_keyValueConfiguration[ApplicationConstants.PublicPort], out int port))
        {
            environmentConfiguration.PublicPort = port;
        }

        if (bool.TryParse(_keyValueConfiguration[ApplicationConstants.PublicPortIsHttps], out bool isHttps))
        {
            environmentConfiguration.PublicPortIsHttps = isHttps;
        }

        if (int.TryParse(_keyValueConfiguration[ApplicationConstants.HttpPort], out int httpPort) && port >= 0)
        {
            environmentConfiguration.HttpPort = httpPort;
        }
        else if (bool.TryParse(_keyValueConfiguration[ApplicationConstants.UseExplicitPorts],
                     out bool useExplicitPorts))
        {
            environmentConfiguration.UseExplicitPorts = useExplicitPorts;
        }

        if (int.TryParse(_keyValueConfiguration[ApplicationConstants.HttpsPort], out int httpsPort) &&
            httpsPort >= 0)
        {
            environmentConfiguration.HttpsPort = httpsPort;
        }

        if (int.TryParse(_keyValueConfiguration[ApplicationConstants.ProxyForwardLimit], out int proxyLimit) &&
            proxyLimit >= 0)
        {
            environmentConfiguration.ForwardLimit = proxyLimit;
        }

        string? pfxFile = _keyValueConfiguration[ApplicationConstants.PfxFile];
        string? pfxPassword = _keyValueConfiguration[ApplicationConstants.PfxPassword];

        if (!string.IsNullOrWhiteSpace(pfxFile) && File.Exists(pfxFile))
        {
            environmentConfiguration.PfxFile = pfxFile;
        }

        if (!string.IsNullOrWhiteSpace(pfxPassword))
        {
            environmentConfiguration.PfxPassword = pfxPassword;
        }
    }
}