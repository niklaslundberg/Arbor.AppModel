﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Arbor.AppModel.Application;
using Arbor.AppModel.Cli;
using Arbor.AppModel.ExtensionMethods;
using Arbor.KVConfiguration.Core;
using Arbor.KVConfiguration.Core.Decorators;
using Arbor.KVConfiguration.JsonConfiguration;
using Arbor.KVConfiguration.UserConfiguration;

namespace Arbor.AppModel.Configuration
{
    public static class ConfigurationInitialization
    {
        public static MultiSourceKeyValueConfiguration InitializeStartupConfiguration(IReadOnlyList<string> args,
            IReadOnlyDictionary<string, string> environmentVariables,
            IReadOnlyCollection<Assembly> assemblies)
        {
            var tempSource = KeyValueConfigurationManager.Add(NoConfiguration.Empty)
                                                         .AddEnvironmentVariables(environmentVariables)
                                                         .AddCommandLineArgsSettings(args).Build();

            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager.Add(NoConfiguration.Empty)
               .AddReflectionSettings(assemblies, tempSource).AddEnvironmentVariables(environmentVariables)
               .AddCommandLineArgsSettings(args).DecorateWith(new ExpandKeyValueConfigurationDecorator()).Build();

            return multiSourceKeyValueConfiguration;
        }

        private static AppSettingsBuilder AddUserSettings(this AppSettingsBuilder builder, string? basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                return builder;
            }

            return builder.Add(new UserJsonConfiguration(basePath));
        }

        private static AppSettingsBuilder AddLoggingSettings(this AppSettingsBuilder builder)
        {
            var loggingSettings = new NameValueCollection
            {
                { "Logging:LogLevel:Default", "Warning" },
                { "Logging:LogLevel:System.Net.Http.HttpClient", "Warning" },
                { "LogLevel:System.Net.Http.HttpClient", "Warning" }
            };

            var memoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(loggingSettings);
            return builder.Add(memoryKeyValueConfiguration);
        }

        private static AppSettingsBuilder AddReflectionSettings(this AppSettingsBuilder appSettingsBuilder,
            IReadOnlyCollection<Assembly>? scanAssemblies,
            IKeyValueConfiguration? configuration = null)
        {
            if (scanAssemblies is null)
            {
                return appSettingsBuilder;
            }

            foreach (var currentAssembly in scanAssemblies.Where(assembly => assembly.FullName is { })
                                                          .OrderBy(assembly => assembly.FullName))
            {
                string[] allValues =
                    configuration?.AllValues.Where(pair => pair.Key.Equals(ApplicationConstants.AssemblyPrefix))
                                  .Select(pair => pair.Value).ToArray() ??
                    [];

                if (allValues.Length > 0 &&
                    !allValues.Any(currentValue => currentAssembly.FullName!.StartsWith(currentValue)))
                {
                    continue;
                }

                try
                {
                    appSettingsBuilder = appSettingsBuilder.Add(new ReflectionKeyValueConfiguration(currentAssembly));
                }
                catch (Exception ex) when (!ex.IsFatal())
                {
                    Debug.WriteLine($"Could not load assembly {currentAssembly.FullName} {ex}");
                }
            }

            return appSettingsBuilder;
        }

        private static AppSettingsBuilder AddSettingsFileFromArgsOrEnvironment(
            this AppSettingsBuilder appSettingsBuilder,
            IReadOnlyList<string>? args,
            IReadOnlyDictionary<string, string>? environmentVariables)
        {
            string? settingsPath = args?.ParseParameter(ConfigurationConstants.JsonSettingsFile) ??
                                   environmentVariables?.ValueOrDefault(ConfigurationConstants.JsonSettingsFile);

            if (settingsPath.HasValue() && File.Exists(settingsPath))
            {
                appSettingsBuilder = appSettingsBuilder.Add(new JsonKeyValueConfiguration(settingsPath));
            }

            return appSettingsBuilder;
        }

        public static MultiSourceKeyValueConfiguration InitializeConfiguration(Func<string?, string>? basePath = null,
            string? contentBasePath = null,
            IReadOnlyCollection<Assembly>? scanAssemblies = null,
            IReadOnlyList<string>? args = null,
            IReadOnlyDictionary<string, string>? environmentVariables = null,
            IKeyValueConfiguration? keyValueConfiguration = null)
        {
            var multiSourceKeyValueConfiguration = KeyValueConfigurationManager.Add(NoConfiguration.Empty)
               .AddReflectionSettings(scanAssemblies, keyValueConfiguration).AddLoggingSettings()
               .AddJsonSettings(basePath, args, environmentVariables).AddMachineSpecificSettings(basePath)
               .AddSettingsFileFromArgsOrEnvironment(args, environmentVariables)
               .AddEnvironmentVariables(environmentVariables).AddUserSettings(contentBasePath)
               .AddCommandLineArgsSettings(args).DecorateWith(new ExpandKeyValueConfigurationDecorator()).Build();

            return multiSourceKeyValueConfiguration;
        }

        public static AppSettingsBuilder AddEnvironmentVariables(this AppSettingsBuilder builder,
            IReadOnlyDictionary<string, string>? environmentVariables)
        {
            if (environmentVariables is null)
            {
                return builder;
            }

            var nameValueCollection = new NameValueCollection();

            foreach (var environmentVariable in environmentVariables)
            {
                nameValueCollection.Add(environmentVariable.Key, environmentVariable.Value);
            }

            return builder.Add(new InMemoryKeyValueConfiguration(nameValueCollection));
        }

        public static AppSettingsBuilder AddCommandLineArgsSettings(this AppSettingsBuilder builder,
            IReadOnlyList<string>? args)
        {
            if (args is null)
            {
                return builder;
            }

            var nameValueCollection = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

            const char variableAssignmentCharacter = '=';

            foreach (string arg in args.Where(currentArg =>
                currentArg.Count(currentChar => currentChar == variableAssignmentCharacter) == 1 &&
                currentArg.Length >= 3))
            {
                string[] parts = arg.Split(variableAssignmentCharacter, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                {
                    continue;
                }

                string key = parts[0];
                string value = parts[1];

                nameValueCollection.Add(key, value);
            }

            var inMemoryKeyValueConfiguration = new InMemoryKeyValueConfiguration(nameValueCollection);
            return builder.Add(inMemoryKeyValueConfiguration);
        }

        public static AppSettingsBuilder AddJsonSettings(this AppSettingsBuilder appSettingsBuilder,
            Func<string, string>? basePath,
            IReadOnlyCollection<string>? args,
            IReadOnlyDictionary<string, string>? environmentVariables)
        {
            if (basePath is null)
            {
                return appSettingsBuilder;
            }

            string environmentName = args?.ParseParameter(ApplicationConstants.AspNetEnvironment) ??
                                     environmentVariables?.ValueOrDefault(ApplicationConstants.AspNetEnvironment) ??
                                     ApplicationConstants.EnvironmentProduction;

            return appSettingsBuilder.Add(new JsonKeyValueConfiguration(basePath("settings.json"), false))
                                     .Add(new JsonKeyValueConfiguration(basePath($"settings.{environmentName}.json"),
                                          false));
        }

        public static AppSettingsBuilder AddMachineSpecificSettings(this AppSettingsBuilder appSettingsBuilder,
            Func<string?, string>? basePath)
        {
            if (basePath is null)
            {
                return appSettingsBuilder;
            }

            static FileInfo? MachineSpecificConfig(DirectoryInfo directoryInfo)
            {
                return directoryInfo.GetFiles($"settings.{Environment.MachineName}.json").SingleOrDefault();
            }

            string? MachineSpecificFile()
            {
                string path = basePath!(null);

                var baseDirectory = new DirectoryInfo(path);

                FileInfo? machineSpecificConfig = null;

                var currentDirectory = baseDirectory;

                while (machineSpecificConfig is null && currentDirectory is { })
                {
                    try
                    {
                        machineSpecificConfig = MachineSpecificConfig(currentDirectory);

                        currentDirectory = currentDirectory.Parent;
                    }
                    catch (Exception ex) when (!ex.IsFatal())
                    {
                        return null;
                    }
                }

                return machineSpecificConfig?.FullName;
            }

            string? machineSpecificFile = MachineSpecificFile();

            if (!string.IsNullOrWhiteSpace(machineSpecificFile))
            {
                appSettingsBuilder = appSettingsBuilder.Add(new JsonKeyValueConfiguration(machineSpecificFile));
            }

            return appSettingsBuilder;
        }
    }
}