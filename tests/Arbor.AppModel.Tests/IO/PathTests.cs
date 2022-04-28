using System.IO;
using Arbor.AppModel.IO;
using FluentAssertions;
using Xunit;

namespace Arbor.AppModel.Tests.IO
{
    public class PathTests
    {
        [Fact]
        public void GetRelativePathShouldReturnPath() => new FileInfo(@"C:\temp\123\abc.txt")
                                                        .GetRelativePath(new DirectoryInfo(@"C:\temp")).Should()
                                                        .Be(@"\123\abc.txt");

        [Fact]
        public void GetRelativePathShouldReturnPathForSubDirectory() => new DirectoryInfo(@"C:\temp\123\abc")
                                                                       .GetRelativePath(new DirectoryInfo(@"C:\temp"))
                                                                       .Should().Be(@"\123\abc");
    }
}