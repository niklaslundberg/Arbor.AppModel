using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.Scheduling;
using Arbor.AppModel.Time;
using Cronos;
using FluentAssertions;
using NCrunch.Framework;
using Serilog;
using Serilog.Core;
using Xunit;
using Xunit.Abstractions;

namespace Arbor.AppModel.Tests
{
    public class ScheduledServiceRealTimerTests : IDisposable
    {
        private readonly Logger _logger;

        public ScheduledServiceRealTimerTests(ITestOutputHelper output)
        {
            var culture = new CultureInfo("sv-SE");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            _logger = new LoggerConfiguration().WriteTo.TestOutput(output, outputTemplate: "{Timestamp:HH:mm:ss.fff} {Level} {Message}{Newline}{Exception}")
                                               .MinimumLevel.Verbose()
                                               .CreateLogger();
        }

        [Isolated]
        [InlineData(1020, 100, 10)]
        [Theory(Skip = "WIP")]
        public async Task ScheduleEvery100MillisecondsWithRealTimer(int timeoutInMilliseconds, int interval, int expected)
        {
            try
            {
                var clock = new CustomSystemClock();
                var start = clock.UtcNow().AddMilliseconds(100);
                var schedule = new ScheduleEveryInterval(TimeSpan.FromMilliseconds(interval), start);
                using var cancellationTokenSource =
                    new CancellationTokenSource(TimeSpan.FromMilliseconds(timeoutInMilliseconds));

                var scheduler = new Scheduler(clock, _logger);
                var cancellationToken = cancellationTokenSource.Token;
                var testService = new TestScheduledService(schedule, scheduler);

                Task CreateAndRunTimer()
                {
                    var systemTimer = new SystemTimer();

                    systemTimer.Register(scheduler);
                    return systemTimer.Run(cancellationToken);

                }

                _ = Task.Run(CreateAndRunTimer, cancellationToken);

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationTokenSource.Token);
                    }
                    catch (Exception)
                    {
                        //
                    }
                }

                await scheduler.DisposeAsync();

                testService.Invokations.Should().Be(expected);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [Isolated]
        [Fact(Skip = "WIP")]
        public async Task ScheduleEverySecondWithRealTimer()
        {
            try
            {
                var clock = new CustomSystemClock();
                var start = clock.UtcNow().AddMilliseconds(200);

                var interval = TimeSpan.FromMilliseconds(1000);
                int cancellationInMilliseconds = 3000;

                var schedule = new ScheduleEveryInterval(interval, start);
                using var cancellationTokenSource =
                    new CancellationTokenSource(TimeSpan.FromMilliseconds(cancellationInMilliseconds));
                using var timer = new SystemTimer(new TimerOptions(TimeSpan.FromMilliseconds(100)), _logger);
                await using var scheduler =
                    new Scheduler(clock, _logger, disposeTimeout: TimeSpan.FromMilliseconds(200));
                var testService = new TestScheduledService(schedule, scheduler);

                timer.Register(scheduler);

                await timer.Run(cancellationTokenSource.Token);
                if (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(cancellationInMilliseconds),
                            cancellationTokenSource.Token);
                    }
                    catch (Exception)
                    {
                        //
                    }
                }

                testService.Invokations.Should().Be(3);
                testService.CompletedInvokations.Should().Be(3);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Isolated]
        [Fact(Skip = "WIP")]
        public async Task ScheduleOverdueTask()
        {
            try
            {
                var clock = new CustomSystemClock();
                var start = clock.UtcNow().AddMilliseconds(100);

                int interval = 100;
                int timeout = 1000;

                var schedule = new ScheduleEveryInterval(TimeSpan.FromMilliseconds(interval), start);
                using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeout));
                using var timer = new SystemTimer(new TimerOptions(TimeSpan.FromMilliseconds(100)), _logger);
                await using var scheduler = new Scheduler(clock, _logger);
                var testService = new DelayScheduledService(TimeSpan.FromMilliseconds(200), schedule, scheduler);

                timer.Register(scheduler);

                await timer.Run(cancellationTokenSource.Token);

                testService.Invokations.Should().Be(0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Isolated]
        [Fact(Skip = "WIP")]
        public async Task ScheduleOverdueTaskAllowed()
        {
            var clock = new CustomSystemClock();
            var start = clock.UtcNow().AddMilliseconds(100);

            int timeout = 1100;

            var scheduledToRunEvery = TimeSpan.FromMilliseconds(100);
            var schedule = new ScheduleEveryInterval(scheduledToRunEvery, start);
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeout));
            using var timer = new SystemTimer(new TimerOptions(TimeSpan.FromMilliseconds(10)), _logger);

            TestScheduledService testService;
            await using (var scheduler = new Scheduler(clock, _logger))
            {
                var iterationRunTime = TimeSpan.FromMilliseconds(200);

                testService =
                    new DelayWithSkipBehaviorScheduledService(iterationRunTime, schedule, scheduler, _logger);

                timer.Register(scheduler);

                await timer.Run(cancellationTokenSource.Token);
            }

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(50));
            }

            testService.Invokations.Should().Be(5);
            testService.CompletedInvokations.Should().Be(5);
        }

        public void Dispose()
        {
            if (_logger is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}