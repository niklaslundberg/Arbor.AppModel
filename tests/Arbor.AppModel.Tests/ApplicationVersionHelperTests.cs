using Arbor.AppModel.Application;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.AppModel.Tests
{
    public class ApplicationVersionHelperTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public ApplicationVersionHelperTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

        [Fact]
        public void Do()
        {
            var applicationVersionInfo = ApplicationVersionHelper.GetAppVersion();

            _outputHelper.WriteLine(applicationVersionInfo?.AssemblyFullName);

            applicationVersionInfo?.AssemblyFullName.Should().NotBeNullOrWhiteSpace();
        }
    }
}