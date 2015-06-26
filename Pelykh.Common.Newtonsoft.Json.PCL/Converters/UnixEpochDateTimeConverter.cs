using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Pelykh.Common.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> to and from a Unix Epoch timestamp (in seconds).
    /// </summary>
    public sealed class UnixEpochDateTimeConverter : DateTimeConverterBase
    {
        public static DateTime EpochOrigin { get; private set; }

        static UnixEpochDateTimeConverter()
        {
            EpochOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var underlyingObjectType = objectType.GetNotNullableTypeOrOriginal();
            var objectTypeIsNullable = (underlyingObjectType != objectType);

            if (reader.TokenType == JsonToken.Null)
            {
                if (!objectTypeIsNullable)
                {
                    throw new JsonSerializationException(string.Format(
                        "Cannot convert null value to {0}",
                        objectType));
                }

                return null;
            }

            if (reader.TokenType != JsonToken.Integer)
            {
                throw new JsonSerializationException(string.Format(
                    "Cannot convert {0} token to {1}",
                    reader.TokenType.ToString(),
                    objectType));
            }

            var seconds = (long)reader.Value;
            var dateTime = EpochOrigin + TimeSpan.FromSeconds(seconds);

            if (underlyingObjectType == typeof(DateTimeOffset))
                return new DateTimeOffset(dateTime);

            return dateTime;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long seconds;

            if (value is DateTime)
            {
                var dateTime = (DateTime)value;
                var utcDateTime = dateTime.ToUniversalTime();
                seconds = (long)(utcDateTime - EpochOrigin).TotalSeconds;
            }
            else if (value is DateTimeOffset)
            {
                var dateTimeOffset = (DateTimeOffset)value;
                var utcDateTimeOffset = dateTimeOffset.ToUniversalTime();
                seconds = (long)(utcDateTimeOffset.UtcDateTime - EpochOrigin).TotalSeconds;
            }
            else
            {
                throw new JsonSerializationException("Expected date object value.");
            }

            writer.WriteValue(seconds);
        }
    }
}
