using System;
using System.Globalization;
using System.Threading.Tasks;
using Arbor.AppModel.Scheduling;
using Cronos;
using FluentAssertions;
using Serilog;
using Serilog.Core;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.AppModel.Tests;

public class ScheduledServiceTests : IDisposable
{
    private readonly Logger _logger;

    public ScheduledServiceTests(ITestOutputHelper output)
    {
        var culture = new CultureInfo("sv-SE");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        _logger = new LoggerConfiguration().WriteTo.TestOutput(output,
                outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level} {Message}{Newline}{Exception}")
            .MinimumLevel.Verbose()
            .CreateLogger();
    }


    public void Dispose()
    {
        if (_logger is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    [Fact]
    public async Task ScheduleOnce()
    {
        var clock = new TestClock(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromSeconds(1));
        var dateTimeOffset = new DateTimeOffset(2000, 1, 1, 0, 0, 4, 0, TimeSpan.Zero);
        var schedule = new ScheduleOnce(dateTimeOffset);
        using var timer = new TestTimer();
        await using var scheduler = new Scheduler(clock, Logger.None);
        var testService = new TestScheduledService(schedule, scheduler);
        timer.Register(scheduler);

        for (int i = 0; i < 8; i++)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await timer.Tick();
        }

        await Task.Delay(TimeSpan.FromMilliseconds(1));

        testService.Invokations.Should().Be(1);
    }

    [Fact]
    public async Task ScheduleEvery4thSeconds()
    {
        var clock = new TestClock(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromSeconds(1));
        var start = new DateTimeOffset(2000, 1, 1, 0, 0, 4, 0, TimeSpan.Zero);
        var schedule = new ScheduleEveryInterval(TimeSpan.FromSeconds(4), start);
        using var timer = new TestTimer();
        await using var scheduler = new Scheduler(clock, _logger);
        var testService = new TestScheduledService(schedule, scheduler);
        timer.Register(scheduler);

        for (int i = 0; i < 10; i++)
        {
            await timer.Tick();
            await Task.Delay(TimeSpan.FromMilliseconds(5));
        }

        await Task.Delay(TimeSpan.FromMilliseconds(1));

        testService.Invokations.Should().Be(2);
    }

    [Fact]
    public async Task ScheduleEverySecond()
    {
        var clock = new TestClock(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero),
            TimeSpan.FromMilliseconds(200));

        var start = new DateTimeOffset(2000, 1, 1, 0, 0, 4, 0, TimeSpan.Zero);
        var schedule = new ScheduleEveryInterval(TimeSpan.FromSeconds(1), start);
        using var timer = new TestTimer();
        await using var scheduler = new Scheduler(clock, Logger.None);

        var testService = new TestScheduledService(schedule, scheduler);
        timer.Register(scheduler);

        for (int i = 0; i < 50; i++)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(10));
            await timer.Tick();
        }

        await Task.Delay(TimeSpan.FromMilliseconds(1));

        testService.Invokations.Should().Be(7);
    }

    [Fact]
    public async Task CronScheduleEvery5Minutes()
    {
        var clock = new TestClock(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromMinutes(1));

        var schedule = new CronSchedule(CronExpression.Parse("*/5 * * * *"));
        using var timer = new TestTimer();
        await using var scheduler = new Scheduler(clock, Logger.None);

        var testService = new TestScheduledService(schedule, scheduler);
        timer.Register(scheduler);

        for (int i = 0; i < 10; i++)
        {
            await timer.Tick();
            await Task.Delay(TimeSpan.FromMilliseconds(1));
        }

        testService.Invokations.Should().Be(2);
    }

    [InlineData(10, 4)]
    [InlineData(100, 49)]
    [Theory]
    public async Task ScheduleEveryOtherSecondSecond(int ticks, int expected)
    {
        var clock = new TestClock(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromSeconds(1));
        var start = new DateTimeOffset(2000, 1, 1, 0, 0, 4, 0, TimeSpan.Zero);
        var schedule = new ScheduleEveryInterval(TimeSpan.FromSeconds(2), start);
        using var timer = new TestTimer();
        await using var scheduler = new Scheduler(clock, Logger.None);
        var testService = new TestScheduledService(schedule, scheduler);
        timer.Register(scheduler);

        for (int i = 0; i < ticks; i++)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            await timer.Tick();
        }

        await Task.Delay(TimeSpan.FromMilliseconds(1));

        testService.Invokations.Should().Be(expected);
    }
}