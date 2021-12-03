using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.ExtensionMethods;
using Arbor.AppModel.Time;
using Serilog;

namespace Arbor.AppModel.Scheduling
{
    public sealed class Scheduler : IScheduler, IAsyncDisposable
    {
        private readonly ICustomClock _clock;
        private readonly ConcurrentDictionary<ScheduledService, DateTimeOffset?> _lastRun = new();
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly ILogger _logger;
        private readonly TimeSpan? _disposeTimeout;

        private readonly ConcurrentDictionary<ScheduledService, Task?> _schedules = new();
        private readonly SemaphoreSlim _tickLock = new(1, 1);
        private bool _isDisposed;
        private bool _isDisposing;
        private bool _isRunning;

        public Scheduler(ICustomClock clock, ILogger logger, TimeSpan? disposeTimeout = null)
        {
            _clock = clock;
            _logger = logger;
            _disposeTimeout = disposeTimeout;
        }

        public bool Add(ScheduledService schedule, OnTickAsync onTick)
        {
            CheckDisposed();

            return _schedules.TryAdd(schedule, default);
        }

        public ImmutableArray<ScheduledService> Schedules => _schedules.Keys.ToImmutableArray();

        public Task Tick(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _cancellationTokenSource.CancelAfter( _disposeTimeout ?? TimeSpan.FromMilliseconds(1000)));

            return OnTickInternal(_clock.UtcNow(), stoppingToken);
        }

        private void CheckDisposed()
        {
            if (_isDisposing || _isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private async Task OnTickInternal(DateTimeOffset currentTime, CancellationToken cancellationToken)
        {
            _logger.Verbose("On tick");
            if (_isRunning)
            {
                _logger.Verbose("Not running yet");
                return;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                _logger.Verbose("Cancellation is requested");
                _isRunning = false;
                return;
            }

            _logger.Verbose("Waiting for tick lock");

            try
            {
                await _tickLock.WaitAsync(cancellationToken);

                if (_isRunning)
                {
                    _logger.Verbose("Still not running");
                    return;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.Verbose("Cancellation is requested now");
                    _isRunning = false;
                    return;
                }

                _isRunning = true;
                _logger.Verbose("Now it is running");

                if (_schedules.IsEmpty)
                {
                    _logger.Verbose("Has no schedules");
                    _isRunning = false;
                    return;
                }

                var toRemove = new List<ScheduledService>();

                foreach (var pair in _schedules)
                {
                    _logger.Verbose("Current time {CurrentTime:HH:mm:ss.fff}", currentTime);
                    var nextTime = pair.Key.Schedule.Next(currentTime);

                    if (nextTime is null)
                    {
                        toRemove.Add(pair.Key);
                        continue;
                    }

                    _logger.Verbose("Next time is {NextTime:HH:mm:ss.fff}, current time {CurrentTime:HH:mm:ss.fff}", nextTime, currentTime);

                    _lastRun.TryGetValue(pair.Key, out var lastRun);

                    bool isCurrentRunning = lastRun.HasValue && lastRun.Value == nextTime;

                    if (isCurrentRunning && pair.Key.SchedulingOptions.SchedulingBehavior == SchedulingBehavior.Skip)
                    {
                        continue;
                    }

                    double maxDiff = pair.Key.SchedulingOptions.SchedulingDelta.Diff.TotalMilliseconds;
                    double absoluteDiff = Math.Abs((currentTime - nextTime).Value.TotalMilliseconds);
                    bool shouldRun = absoluteDiff >= 0 && absoluteDiff <= maxDiff;

                    if (!shouldRun)
                    {
                        _logger.Verbose("Current time {CurrentTime:HH:mm:ss.fff} is < next {NextTime:HH:mm:ss.fff}",
                            currentTime,
                            nextTime);
                    }

                    if (_schedules.TryGetValue(pair.Key, out var task) && task is {IsCompleted: false } )
                    {
                        if (pair.Key.SchedulingOptions.SchedulingBehavior == SchedulingBehavior.Skip)
                        {
                            _logger.Verbose("A scheduled service is already running and hos not completed");
                           shouldRun = false;
                        }

                    }
                    else if (task is {IsCompleted: true})
                    {
                        _schedules[pair.Key] = default;
                    }

                    if (nextTime >= lastRun)
                    {
                        //shouldRun = false;
                    }

                    if (shouldRun)
                    {
                        Run(pair.Key, currentTime, nextTime);
                    }
                    else
                    {
                        _logger.Verbose("Skipping run");
                    }

                    /*else if (nextTime > lastRun && pair.Key)*/

                    //double diff = (currentTime - nextTime.Value).TotalMilliseconds;

                    //double absoluteDiff = Math.Abs(diff);

                    //using (var cancellationTokenSource = pair.Key.AllowOverdue ? new CancellationTokenSource() : new CancellationTokenSource(TimeSpan.FromMilliseconds(absoluteDiff)))
                    //{
                    //    using var combined =
                    //        CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token,
                    //            cancellationToken);

                    //    if (absoluteDiff < 50 &&
                    //        _lastRun.TryAdd(pair.Key, nextTime) &&
                    //        (!pair.Key.AllowOverdue ||
                    //         pair.Value is null or { IsCompleted: false }))
                    //    {
                    //
                    //    }
                    //    else if (nextTime > lastRun && !pair.Key.AllowOverdue)
                    //    {
                    //        _lastRun.TryRemove(pair.Key, out _);
                    //    }
                    //}
                }

                foreach (var schedule in toRemove)
                {
                    _schedules.TryRemove(schedule, out _);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.Debug("Cancellation is requested, stopping schedules");
                    _isRunning = false;
                }

                _isRunning = false;
            }
            finally
            {
                _tickLock.Release();
            }
        }

        private void Run(ScheduledService scheduledService,
            DateTimeOffset currentTime,
            DateTimeOffset? nextTime)
        {
            _lastRun.TryRemove(scheduledService, out _);
            _lastRun.TryAdd(scheduledService, nextTime);
            _logger.Information("Running schedule {Schedule:HH:mm:ss.fff} at {CurrentTime:HH:mm:ss.fff}", scheduledService, currentTime);

            var task = scheduledService.RunAsync(currentTime, _cancellationTokenSource.Token);
            _schedules[scheduledService] = task;
            _ = Task.Run(async () => await task, _cancellationTokenSource.Token);

        }

        public async ValueTask DisposeAsync()
        {
            if (_isDisposed || _isDisposing)
            {
                return;
            }

            if (!_schedules.IsEmpty && _schedules.Values is {} tasks)
            {
                await Task.WhenAll(tasks.NotNull());
            }

            _isDisposing = true;

            _schedules.Clear();

            _cancellationTokenSource.Dispose();

            _isDisposed = true;
        }
    }
}