using Arbor.AppModel.Application;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.AppModel.Tests;

public class ApplicationVersionHelperTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Do()
    {
        var applicationVersionInfo = ApplicationVersionHelper.GetAppVersion();

        outputHelper.WriteLine(applicationVersionInfo?.AssemblyFullName);

        applicationVersionInfo?.AssemblyFullName.Should().NotBeNullOrWhiteSpace();
    }
}