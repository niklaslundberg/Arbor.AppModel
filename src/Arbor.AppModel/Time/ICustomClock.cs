using System;

namespace Arbor.AppModel.Time
{
    public interface ICustomClock
    {
        TimeZoneInfo DefaultTimeZone { get; }

        DateTimeOffset UtcNow();

        DateTime LocalNow();

        DateTime ToLocalTime(DateTime dateTimeUtc);
    }
}