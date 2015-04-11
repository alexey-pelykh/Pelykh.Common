using NodaTime;
using System;

namespace Pelykh.Common.NodaTime
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertToTimeZone(this DateTime dateTime, string timeZoneId)
        {
            timeZoneId.ThrowIfNull("timeZoneId");

            var timeZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            return dateTime.ConvertToTimeZone(timeZone);
        }

        public static DateTime ConvertToTimeZone(this DateTime dateTime, DateTimeZone timeZone)
        {
            timeZone.ThrowIfNull("timeZone");

            if (dateTime.Kind != DateTimeKind.Utc)
                throw new InvalidOperationException("Source DateTime must be in UTC");

            if (timeZone == DateTimeZone.Utc)
                return dateTime;

            return Instant.FromDateTimeUtc(dateTime).InZone(timeZone).ToDateTimeUnspecified();
        }

        public static DateTimeOffset ConvertToTimeZoneWithOffset(this DateTime dateTime, string timeZoneId)
        {
            timeZoneId.ThrowIfNull("timeZoneId");

            var timeZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            return dateTime.ConvertToTimeZoneWithOffset(timeZone);
        }

        public static DateTimeOffset ConvertToTimeZoneWithOffset(this DateTime dateTime, DateTimeZone timeZone)
        {
            timeZone.ThrowIfNull("timeZone");

            if (dateTime.Kind != DateTimeKind.Utc)
                throw new InvalidOperationException("Source DateTime must be in UTC");

            if (timeZone == DateTimeZone.Utc)
                return dateTime;

            return Instant.FromDateTimeUtc(dateTime).InZone(timeZone).ToDateTimeOffset();
        }

        public static DateTime ConvertToUtcFromTimeZone(this DateTime dateTime, string timeZoneId)
        {
            timeZoneId.ThrowIfNull("timeZoneId");

            var timeZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            return dateTime.ConvertToUtcFromTimeZone(timeZone);
        }

        public static DateTime ConvertToUtcFromTimeZone(this DateTime dateTime, DateTimeZone timeZone)
        {
            timeZone.ThrowIfNull("timeZone");

            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;

            return timeZone.AtLeniently(LocalDateTime.FromDateTime(dateTime)).WithZone(DateTimeZone.Utc).ToDateTimeUtc();
        }
    }
}
