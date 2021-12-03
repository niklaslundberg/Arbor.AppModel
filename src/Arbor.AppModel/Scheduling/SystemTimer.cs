using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace Arbor.AppModel.Scheduling
{
    public sealed class SystemTimer : ITimer
    {
        private readonly TimerOptions _timerOptions;
        private readonly ILogger _logger;
        private readonly List<IScheduler> _schedulers = new();

        private PeriodicTimer? _timer;

        private bool _disposed;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private bool _isRunning;

        public SystemTimer(TimerOptions? timerOptions = default, ILogger? logger = default)
        {
            _timerOptions = timerOptions ?? new TimerOptions(TimeSpan.FromMilliseconds(10));
            _cancellationTokenSource = new CancellationTokenSource();
            _logger = logger ?? Logger.None;
        }

        public void Register(IScheduler scheduler)
        {
            CheckDisposed();

            _schedulers.Add(scheduler);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _schedulers.Clear();
            _cancellationTokenSource.Cancel();
            _timer?.Dispose();
            _timer = null;
            _disposed = true;
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            if (_cancellationTokenSource.IsCancellationRequested)
            {
                throw new TaskCanceledException("Timer cancelled");
            }
        }


        public async Task Run(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _cancellationTokenSource.Cancel());

            if (_isRunning)
            {
                return;
            }

            _timer = new PeriodicTimer(_timerOptions.Period);
            _isRunning = true;

            _logger.Verbose("Starting timer");

            try
            {
                while (_timer is {}
                       && await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token)
                       && !_cancellationTokenSource.IsCancellationRequested)
                {
                    CheckDisposed();

                    if (_schedulers.Count == 0)
                    {
                        return;
                    }

                    foreach (var scheduler in _schedulers)
                    {
                        try
                        {
                            _logger.Verbose("Invoking scheduler {Scheduler}", scheduler);
                            await scheduler.Tick(_cancellationTokenSource.Token);
                            _logger.Verbose("Invoked scheduler {Scheduler}", scheduler);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex, "Timer invokation failed");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Verbose("Timer stopped with operation canceled");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Timer failed");
            }
        }
    }
}