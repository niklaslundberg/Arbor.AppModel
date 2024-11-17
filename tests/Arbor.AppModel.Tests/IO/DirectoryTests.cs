using System;
using System.IO;
using Arbor.AppModel.IO;
using FluentAssertions;
using Xunit;

namespace Arbor.AppModel.Tests.IO;

public class DirectoryTests
{
    [Fact]
    public void TryEnsureDirectoryExists()
    {
        DirectoryInfo? dir = null;

        try
        {
            dir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            bool succeeded = dir.TryEnsureDirectoryExists(out var created);

            succeeded.Should().BeTrue();

            created.Should().NotBeNull();
        }
        finally
        {
            if (dir?.Exists == true)
            {
                dir.Delete(true);
            }
        }
    }

    [Fact]
    public void EnsureDirectoryExists()
    {
        DirectoryInfo? dir = null;

        bool exists;

        try
        {
            dir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())).EnsureExists();

            exists = Directory.Exists(dir.FullName);
        }
        finally
        {
            if (dir?.Exists == true)
            {
                dir.Delete(true);
            }
        }

        exists.Should().BeTrue();
    }
}