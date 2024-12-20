﻿using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Arbor.AppModel.Time
{
    public static class DateTimeExtensions
    {
        public static RelativeInterval IntervalAgo(this DateTime? dateTimeUtc, TimeProvider customClock)
        {
            ArgumentNullException.ThrowIfNull(customClock);

            if (!dateTimeUtc.HasValue)
            {
                return RelativeInterval.Invalid;
            }

            var diff = customClock.GetLocalNow() - dateTimeUtc.Value;

            if (diff.TotalSeconds < 0)
            {
                return RelativeInterval.Invalid;
            }

            return RelativeInterval.Parse(diff);
        }

        public static string RelativeUtcToLocalTime(this DateTime? dateTimeUtc, TimeProvider customClock)
        {
            ArgumentNullException.ThrowIfNull(customClock);

            if (!dateTimeUtc.HasValue)
            {
                return Constants.NotAvailable;
            }

            var localThen = TimeZoneInfo.ConvertTime(dateTimeUtc.Value, customClock.LocalTimeZone);

            var localNow = customClock.GetLocalNow().LocalDateTime;

            return localNow.Since(localThen);
        }

        public static string ToLocalTimeFormatted(this DateTime? dateTimeUtc, TimeProvider customClock)
        {
            ArgumentNullException.ThrowIfNull(customClock);

            return !dateTimeUtc.HasValue
                ? ""
                : ToLocalTimeFormatted(dateTimeUtc.Value, customClock);
        }

        public static string ToLocalTimeFormatted(this DateTime dateTimeUtc, TimeProvider customClock)
        {
            ArgumentNullException.ThrowIfNull(customClock);

            return TimeZoneInfo.ConvertTime(dateTimeUtc, customClock.LocalTimeZone)
                .ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentUICulture);
        }

        [PublicAPI]
        public static string Since(this DateTime to, DateTime from)
        {
            static string PluralSuffix(int count)
            {
                return count > 1 ? "s" : "";
            }

            var diff = to - from;

            int diffTotalDays = (int)diff.TotalDays;

            if (diff.TotalDays > 365)
            {
                return $"{diffTotalDays} day{PluralSuffix(diffTotalDays)} ago";
            }

            if (diff.TotalDays > 30)
            {
                int totalMonths = diffTotalDays / 30;
                return $"{totalMonths} month{PluralSuffix(totalMonths)} ago";
            }

            if (diff.TotalDays > 1)
            {
                return $"{diffTotalDays} day{PluralSuffix(diffTotalDays)} ago";
            }

            if (diff.TotalHours > 1)
            {
                int diffTotalHours = (int)diff.TotalHours;
                return $"{diffTotalHours} hour{PluralSuffix(diffTotalHours)} ago";
            }

            if (diff.TotalMinutes > 1)
            {
                int diffTotalMinutes = (int)diff.TotalMinutes;
                return $"{diffTotalMinutes} minute{PluralSuffix(diffTotalMinutes)} ago";
            }

            if (diff.TotalSeconds < 0)
            {
                return Constants.NotAvailable;
            }

            int diffTotalSeconds = (int)diff.TotalSeconds;

            return $"{diffTotalSeconds} second{PluralSuffix(diffTotalSeconds)} ago";
        }

        public static string ToLocalDateTimeFormat(this DateTimeOffset? dateTimeOffset,
            TimeProvider clock,
            string? format = null)
        {
            if (dateTimeOffset is null)
            {
                return "";
            }

            var localTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.Value.UtcDateTime, clock.LocalTimeZone);

            return localTime.ToString(format ?? "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
    }
}