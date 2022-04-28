using System;
using System.IO;
using System.Reflection;
using Arbor.AppModel.Application;
using Arbor.KVConfiguration.Core;
using Serilog;

namespace Arbor.AppModel.IO
{
    public static class TempPathHelper
    {
        public static void SetTempPath(MultiSourceKeyValueConfiguration configuration, ILogger logger)
        {
            string tempDirectory = configuration[ApplicationConstants.ApplicationTempDirectory];

            if (!string.IsNullOrWhiteSpace(tempDirectory))
            {
                SetTempPath(new DirectoryInfo(tempDirectory), logger);
            }
        }

        public static void SetTempPath(DirectoryInfo directoryInfo, ILogger startupLogger)
        {
            if (directoryInfo.TryEnsureDirectoryExists(out var tempDirectoryInfo) && tempDirectoryInfo is { })
            {
                Environment.SetEnvironmentVariable(TempConstants.Tmp, tempDirectoryInfo.FullName);
                Environment.SetEnvironmentVariable(TempConstants.Temp, tempDirectoryInfo.FullName);

                startupLogger.Debug("Using specified temp directory {TempDirectory} {AppName}",
                    directoryInfo.FullName,
                    Assembly.GetExecutingAssembly().GetName().Name);
            }
            else
            {
                startupLogger.Warning("Could not use specified temp directory {TempDirectory}, {AppName}",
                    directoryInfo.FullName,
                    Assembly.GetExecutingAssembly().GetName().Name);
            }
        }
    }
}