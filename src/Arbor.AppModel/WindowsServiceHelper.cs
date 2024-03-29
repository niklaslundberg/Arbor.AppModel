﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Arbor.AppModel.Application;
using Arbor.AppModel.ExtensionMethods;

namespace Arbor.AppModel
{
    public static class WindowsServiceHelper
    {
        public static bool IsRunningAsService(IReadOnlyCollection<string> commandLineArgs)
        {
            bool hasRunAsServiceArgument = commandLineArgs.Any(arg =>
                arg.Equals(ApplicationConstants.RunAsService, StringComparison.OrdinalIgnoreCase));

            if (hasRunAsServiceArgument)
            {
                return true;
            }

            try
            {
                using var currentProcess = Process.GetCurrentProcess();

                if (currentProcess.MainModule is null)
                {
                    throw new InvalidOperationException("The main module for the current process could not be found");
                }

                return currentProcess.StartInfo.ArgumentList.Contains(ApplicationConstants.RunAsService,
                    StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex) when (!ex.IsFatal())
            {
                // ignore
            }

            return !Environment.UserInteractive;
        }
    }
}