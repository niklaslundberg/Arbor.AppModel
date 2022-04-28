using System;
using Arbor.AppModel.Time;

namespace Arbor.AppModel.Tests
{
    public class TestClock : ICustomClock
    {
        private readonly TimeSpan _tickDuration;
        private DateTimeOffset _dateTimeOffset;

        public TestClock(DateTimeOffset dateTimeOffset, TimeSpan tickDuration)
        {
            _dateTimeOffset = dateTimeOffset;
            _tickDuration = tickDuration;
        }

        public DateTimeOffset UtcNow()
        {
            _dateTimeOffset = _dateTimeOffset.Add(_tickDuration);

            return _dateTimeOffset;
        }

        public DateTime LocalNow() => throw new NotSupportedException();

        public DateTime ToLocalTime(DateTime dateTimeUtc) => throw new NotSupportedException();

        public TimeZoneInfo DefaultTimeZone { get; } = TimeZoneInfo.Utc;
    }
}