using System;
using System.Linq;
using Arbor.AppModel.ExtensionMethods;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.AppModel.Time
{
    [UsedImplicitly]
    public class CustomSystemClock : ICustomClock
    {
        public CustomSystemClock(IKeyValueConfiguration? keyValueConfiguration = null, string? timeZoneId = null)
        {
            timeZoneId = !string.IsNullOrWhiteSpace(timeZoneId)
                ? timeZoneId
                : keyValueConfiguration?[TimeConstants.DefaultTimeZoneId];

            if (timeZoneId.HasValue())
            {
                var foundTimeZone = TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(zone =>
                    zone.Id.Equals(timeZoneId, StringComparison.OrdinalIgnoreCase));

                if (foundTimeZone != null)
                {
                    DefaultTimeZone = foundTimeZone;
                    return;
                }
            }

            DefaultTimeZone = TimeZoneInfo.Utc;
        }

        public DateTimeOffset UtcNow() => ReferenceEquals(DefaultTimeZone, TimeZoneInfo.Utc)
            ? DateTimeOffset.UtcNow
            : TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, DefaultTimeZone.Id);

        public DateTime LocalNow() => ReferenceEquals(DefaultTimeZone, TimeZoneInfo.Utc)
            ? DateTime.UtcNow
            : TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, DefaultTimeZone);

        public DateTime ToLocalTime(DateTime dateTimeUtc) =>
            TimeZoneInfo.ConvertTimeFromUtc(new DateTime(dateTimeUtc.Ticks, DateTimeKind.Utc), DefaultTimeZone);

        public TimeZoneInfo DefaultTimeZone { get; }
    }
}