﻿using System;
using System.IO;
using Arbor.App.Extensions.IO;
using FluentAssertions;
using Serilog.Core;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.App.Extensions.Tests.IO
{
    public class TempFileTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TempFileTests(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public void UsingTempFile()
        {
            using var tempFile = TempFile.CreateTempFile();

            _testOutputHelper.WriteLine(tempFile.File?.FullName);

            tempFile.File.Should().NotBeNull();

            tempFile.File!.Exists.Should().BeTrue();
        }

        [Fact]
        public void UsingTempFileWithName()
        {
            using var tempFile = TempFile.CreateTempFile("Abc");

            _testOutputHelper.WriteLine(tempFile.File?.FullName);

            tempFile.File.Should().NotBeNull();

            tempFile.File!.Exists.Should().BeTrue();
        }

        [Fact]
        public void UsingTempFileCustomTempDirectory()
        {
            using var tempFile = TempFile.CreateTempFile(tempDirectory: new DirectoryInfo(Path.GetTempPath()));

            _testOutputHelper.WriteLine(tempFile.File?.FullName);

            tempFile.File.Should().NotBeNull();

            tempFile.File!.Exists.Should().BeTrue();
        }

        [Fact]
        public void UsingTempFileCustomTempDirectoryAndCustomName()
        {
            using var tempFile =
                TempFile.CreateTempFile("Abc123", tempDirectory: new DirectoryInfo(Path.GetTempPath()));

            _testOutputHelper.WriteLine(tempFile.File?.FullName);

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

                _testOutputHelper.WriteLine(directory);
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
}