using JetBrains.Annotations;

namespace Arbor.AppModel.HealthChecks
{
    public class HealthCheckResult
    {
        public HealthCheckResult(bool succeeded) => Succeeded = succeeded;

        [PublicAPI]
        public bool Succeeded { get; }
    }
}