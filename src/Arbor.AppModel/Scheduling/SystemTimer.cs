using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.ExtensionMethods;
using Serilog;
using Serilog.Core;

namespace Arbor.AppModel.Scheduling;

public sealed class SystemTimer : ITimer
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ILogger _logger;
    private readonly List<IScheduler> _schedulers = new();
    private readonly TimerOptions _timerOptions;

    private bool _disposed;
    private bool _isDisposing;
    private bool _isRunning;

    private PeriodicTimer? _timer;

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
        if (_disposed || _isDisposing)
        {
            return;
        }

        _isDisposing = true;


        _schedulers.Clear();
        _cancellationTokenSource.Cancel();
        _timer.SafeDispose();
        _timer = null;
        _cancellationTokenSource.SafeDispose();
        _disposed = true;
        _isDisposing = false;
    }


    public async Task Run(CancellationToken stoppingToken)
    {
        if (_isRunning)
        {
            return;
        }

        using var combinedToken =
            CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _cancellationTokenSource.Token);

        _timer = new PeriodicTimer(_timerOptions.Period);
        _isRunning = true;

        _logger.Verbose("Starting timer");

        try
        {
            while (!combinedToken.IsCancellationRequested && !_disposed && !_isDisposing && _timer is { }
                   && await _timer.WaitForNextTickAsync(combinedToken.Token))
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
                        await scheduler.Tick(combinedToken.Token);
                        _logger.Verbose("Invoked scheduler {Scheduler}", scheduler);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Timer invocation failed");
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

    private void CheckDisposed()
    {
        if (_disposed || _isDisposing)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }

        if (_cancellationTokenSource.IsCancellationRequested)
        {
            throw new TaskCanceledException("Timer cancelled");
        }
    }
}