using System;
using System.Linq;
using Arbor.App.Extensions.ExtensionMethods;
using Arbor.KVConfiguration.Core;
using JetBrains.Annotations;

namespace Arbor.App.Extensions.Time
{
    [UsedImplicitly]
    public class CustomSystemClock : ICustomClock
    {
        private readonly TimeZoneInfo _timeZone;

        public CustomSystemClock(
            IKeyValueConfiguration? keyValueConfiguration = null,
            string? timeZoneId = null)
        {
            timeZoneId = !string.IsNullOrWhiteSpace(timeZoneId)
                ? timeZoneId
                : keyValueConfiguration?[
                    TimeConstants.DefaultTimeZoneId];

            if (timeZoneId.HasValue())
            {
                var foundTimeZone = TimeZoneInfo.GetSystemTimeZones()
                    .SingleOrDefault(zone => zone.Id.Equals(timeZoneId, StringComparison.OrdinalIgnoreCase));

                if (foundTimeZone != null)
                {
                    _timeZone = foundTimeZone;
                    return;
                }
            }

            _timeZone = TimeZoneInfo.Utc;
        }

        public DateTimeOffset UtcNow() => ReferenceEquals(_timeZone, TimeZoneInfo.Utc)
            ? DateTimeOffset.UtcNow
            : TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, _timeZone.Id);

        public DateTime LocalNow() => ReferenceEquals(_timeZone, TimeZoneInfo.Utc)
            ? DateTime.UtcNow
            : TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);

        public DateTime ToLocalTime(DateTime dateTimeUtc) =>
            TimeZoneInfo.ConvertTimeFromUtc(new DateTime(dateTimeUtc.Ticks, DateTimeKind.Utc), _timeZone);

        public TimeZoneInfo DefaultTimeZone => _timeZone;
    }
}