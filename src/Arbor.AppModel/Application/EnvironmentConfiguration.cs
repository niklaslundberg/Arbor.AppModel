﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using Arbor.AppModel.Configuration;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;

namespace Arbor.AppModel.Application;

public class EnvironmentConfiguration : IConfigurationValues
{
    public bool UseVerboseExceptions =>
        !EnvironmentName.HasValue() ||
        !EnvironmentName.Equals(ApplicationConstants.EnvironmentProduction, StringComparison.OrdinalIgnoreCase);

    [PublicAPI]
    public string? ApplicationBasePath { get; set; }

    [PublicAPI]
    public string? ContentBasePath { get; set; }

    [PublicAPI]
    public string? EnvironmentName { get; set; }

    [PublicAPI]
    public string? ApplicationName { get; set; }

    [PublicAPI]
    public string? PublicHostname { get; set; }

    public bool UseExplicitPorts { get; set; } = true;

    [PublicAPI]
    public bool HttpEnabled { get; set; }

    [PublicAPI]
    public int? PublicPort { get; set; }

    [PublicAPI]
    public bool? PublicPortIsHttps { get; set; }

    [PublicAPI]
    public int? HttpPort { get; set; } = 34343;

    [PublicAPI]
    public int? HttpsPort { get; set; }

    [PublicAPI]
    public string? PfxFile { get; set; }

    [PublicAPI]
    public string? PfxPassword { get; set; }

    public bool UseVerboseLogging { get; set; }

    public bool IsDevelopmentMode { get; set; }

    public int? ForwardLimit { get; set; }

    public ImmutableArray<string> CommandLineArgs { get; set; } = ImmutableArray<string>.Empty;

    public List<IPAddress> ProxyAddresses { get; } = [];

    public override string ToString() =>
        $"{nameof(ApplicationBasePath)}: {ApplicationBasePath}, {nameof(ContentBasePath)}: {ContentBasePath}, {nameof(EnvironmentName)}: {EnvironmentName}, {nameof(HttpPort)}: {HttpPort}, {nameof(HttpsPort)}: {HttpsPort}, {nameof(PfxFile)}: {PfxFile}, {nameof(PfxPassword)}: *****";
}