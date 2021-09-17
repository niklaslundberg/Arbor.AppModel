using System;

namespace Arbor.App.Extensions.Time
{
    public interface ICustomClock
    {
        TimeZoneInfo DefaultTimeZone { get; }

        DateTimeOffset UtcNow();

        DateTime LocalNow();

        DateTime ToLocalTime(DateTime dateTimeUtc);
    }
}