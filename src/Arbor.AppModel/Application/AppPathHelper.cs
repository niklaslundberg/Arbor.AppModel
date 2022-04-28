using System;
using System.Collections.Generic;
using System.IO;
using Arbor.AppModel.IO;
using Arbor.AppModel.Logging;

namespace Arbor.AppModel.Application
{
    public static class AppPathHelper
    {
        public static void SetApplicationPaths(ApplicationPaths paths, IReadOnlyCollection<string> commandLineArgs)
        {
            string? currentDomainBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (WindowsServiceHelper.IsRunningAsService(commandLineArgs))
            {
                TempLogger.WriteLine(
                    $"Switching current directory from {Directory.GetCurrentDirectory()} to {currentDomainBaseDirectory}");

                Directory.SetCurrentDirectory(currentDomainBaseDirectory);
            }

            paths.BasePath ??= currentDomainBaseDirectory;
            paths.ContentBasePath ??= Directory.GetCurrentDirectory();
        }
    }
}