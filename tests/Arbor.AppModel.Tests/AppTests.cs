using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Logging;
using FluentAssertions;
using FluentAssertions.Extensions;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.AppModel.Tests;

public class AppTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public async Task RunAppShouldReturnExitCode0()
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        object[] instances = [new TestDependency()];

        var appTask = Task.Run(() => AppStarter<TestStartup>.StartAsync([],
            new Dictionary<string, string>(),
            cancellationTokenSource,
            instances: instances));

        await Task.Delay(1.Seconds());
        cancellationTokenSource.Cancel();

        int appExitCode = await appTask;

        using var logger = outputHelper.CreateTestLogger();

        TempLogger.FlushWith(logger);

        appExitCode.Should().Be(0);
    }
}