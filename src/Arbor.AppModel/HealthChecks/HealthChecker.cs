using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Arbor.AppModel.ExtensionMethods;
using Arbor.AppModel.Time;
using Serilog;

namespace Arbor.AppModel.HealthChecks;

public class HealthChecker(
    IEnumerable<IHealthCheck> healthChecks,
    ILogger logger,
    TimeoutHelper timeoutHelper)
{
    private readonly ImmutableArray<IHealthCheck> _healthChecks = healthChecks.SafeToImmutableArray();
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task PerformHealthChecksAsync(CancellationToken cancellationToken)
    {
        if (_healthChecks.Length == 0)
        {
            _logger.Debug("No health checks are registered");
            return;
        }

        _logger.Debug("{HealthCheckCount} health checks are registered", _healthChecks.Length);

        foreach (IHealthCheck healthCheck in _healthChecks)
        {
            try
            {
                using CancellationTokenSource cts =
                    timeoutHelper.CreateCancellationTokenSource(
                        TimeSpan.FromSeconds(healthCheck.TimeoutInSeconds));

                using var combined = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                _logger.Debug("Making health check with {Check}", healthCheck.Description);
                await healthCheck.CheckHealthAsync(combined.Token);
            }
            catch (Exception ex) when (!ex.IsFatal())
            {
                _logger.Debug(ex, "Health check error for check {Check}", healthCheck.Description);
            }
        }

        _logger.Debug("Health checks done");
    }
}