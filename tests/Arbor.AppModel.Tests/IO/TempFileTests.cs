using System;
using System.IO;
using Arbor.AppModel.IO;
using FluentAssertions;
using Serilog.Core;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.AppModel.Tests.IO;

public class TempFileTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void UsingTempFile()
    {
        using var tempFile = TempFile.CreateTempFile();

        testOutputHelper.WriteLine(tempFile.File?.FullName);

        tempFile.File.Should().NotBeNull();

        tempFile.File!.Exists.Should().BeTrue();
    }

    [Fact]
    public void UsingTempFileWithName()
    {
        using var tempFile = TempFile.CreateTempFile("Abc");

        testOutputHelper.WriteLine(tempFile.File?.FullName);

        tempFile.File.Should().NotBeNull();

        tempFile.File!.Exists.Should().BeTrue();
    }

    [Fact]
    public void UsingTempFileCustomTempDirectory()
    {
        using var tempFile = TempFile.CreateTempFile(tempDirectory: new DirectoryInfo(Path.GetTempPath()));

        testOutputHelper.WriteLine(tempFile.File?.FullName);

        tempFile.File.Should().NotBeNull();

        tempFile.File!.Exists.Should().BeTrue();
    }

    [Fact]
    public void UsingTempFileCustomTempDirectoryAndCustomName()
    {
        using var tempFile =
            TempFile.CreateTempFile("Abc123", tempDirectory: new DirectoryInfo(Path.GetTempPath()));

        testOutputHelper.WriteLine(tempFile.File?.FullName);

        tempFile.File.Should().NotBeNull();

        tempFile.File!.Exists.Should().BeTrue();
    }

    [Fact]
    public void SetTempDirectory()
    {
        string original = Path.GetTempPath();

        try
        {
            string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            testOutputHelper.WriteLine(directory);
            var directoryInfo = new DirectoryInfo(directory);
            TempPathHelper.SetTempPath(directoryInfo, Logger.None);

            Path.GetTempPath().TrimEnd('\\').Should().Be(directory.TrimEnd('\\'));

            directoryInfo.Delete(true);
        }
        finally
        {
            TempPathHelper.SetTempPath(new DirectoryInfo(original), Logger.None);
        }
    }
}