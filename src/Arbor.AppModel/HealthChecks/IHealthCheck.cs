using System.Threading;
using System.Threading.Tasks;

namespace Arbor.AppModel.HealthChecks
{
    public interface IHealthCheck
    {
        int TimeoutInSeconds { get; }

        string Description { get; }

        Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken);
    }
}