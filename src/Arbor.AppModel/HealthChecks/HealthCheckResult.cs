using JetBrains.Annotations;

namespace Arbor.AppModel.HealthChecks;

public class HealthCheckResult(bool succeeded)
{
    [PublicAPI]
    public bool Succeeded { get; } = succeeded;
}