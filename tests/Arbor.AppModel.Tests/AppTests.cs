using System;
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

public class AppTests(ITestOutputHelper outputHelper) : IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;

    [Fact]
    public async Task RunAppShouldReturnExitCode0()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        object[] instances = [new TestDependency()];

        var appTask = Task.Run(() => AppStarter<TestStartup>.StartAsync([],
            new Dictionary<string, string>(),
            _cancellationTokenSource,
            instances: instances));

        await Task.Delay(1.Seconds(), CancellationToken.None);
        await _cancellationTokenSource.CancelAsync();

        int appExitCode = await appTask;

        await using var logger = outputHelper.CreateTestLogger();

        TempLogger.FlushWith(logger);

        appExitCode.Should().Be(0);
    }

    public void Dispose() => _cancellationTokenSource.Dispose();
}