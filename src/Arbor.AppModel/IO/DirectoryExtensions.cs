using System;
using System.IO;
using Arbor.AppModel.ExtensionMethods;
using JetBrains.Annotations;

namespace Arbor.AppModel.IO
{
    public static class DirectoryExtensions
    {
        public static bool
            TryEnsureDirectoryExists(this string directory, out DirectoryInfo? directoryInfo) =>
            TryEnsureDirectoryExists(new DirectoryInfo(directory), out directoryInfo);

        public static bool TryEnsureDirectoryExists(this DirectoryInfo directory,
            out DirectoryInfo? directoryInfo)
        {
            if (directory.FullName is null)
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(directory));
            }

            try
            {
                directoryInfo = directory.EnsureExists();

                return true;
            }
            catch (Exception ex) when (!ex.IsFatal())
            {
                directoryInfo = null;
                return false;
            }
        }

        public static DirectoryInfo EnsureExists(this DirectoryInfo directoryInfo)
        {
            ArgumentNullException.ThrowIfNull(directoryInfo);

            directoryInfo.Refresh();

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            directoryInfo.Refresh();

            return directoryInfo;
        }
    }
}