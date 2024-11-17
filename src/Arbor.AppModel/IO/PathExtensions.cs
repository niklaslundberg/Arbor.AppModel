using System;
using System.IO;
using JetBrains.Annotations;

namespace Arbor.AppModel.IO
{
    public static class PathExtensions
    {
        private static string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            }

            return path.Trim(Path.DirectorySeparatorChar);
        }

        public static string GetRelativePath(this FileInfo file, DirectoryInfo rootPath)
        {
            ArgumentNullException.ThrowIfNull(file);

            ArgumentNullException.ThrowIfNull(rootPath);

            string rootFullPath = NormalizePath(rootPath.FullName);
            string fullPath = NormalizePath(file.FullName);

            return GetRelativePath(fullPath, rootFullPath);
        }

        public static string GetRelativePath(this DirectoryInfo directoryInfo,
            DirectoryInfo rootPath)
        {
            ArgumentNullException.ThrowIfNull(directoryInfo);

            ArgumentNullException.ThrowIfNull(rootPath);

            string rootFullPath = NormalizePath(rootPath.FullName);
            string fullPath = NormalizePath(directoryInfo.FullName);

            return GetRelativePath(fullPath, rootFullPath);
        }

        private static string GetRelativePath(this string fullPath, string rootFullPath)
        {
            if (!fullPath.StartsWith(rootFullPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Could not find rootPath in fullPath when calculating relative path.");
            }

            return fullPath[rootFullPath.Length..];
        }
    }
}