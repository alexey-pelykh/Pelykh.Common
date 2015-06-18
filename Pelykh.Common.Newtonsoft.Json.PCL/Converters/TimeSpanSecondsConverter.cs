using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Pelykh.Common.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="TimeSpan"/> to and from a value in seconds.
    /// </summary>
    public sealed class TimeSpanSecondsConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long seconds;

            if (value is TimeSpan)
            {
                var timeSpan = (TimeSpan)value;
                seconds = (long)timeSpan.TotalSeconds;
            }
            else
            {
                throw new JsonSerializationException("Expected TimeSpan object value.");
            }

            writer.WriteValue(seconds);
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

            return TimeSpan.FromSeconds(seconds);
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?))
                return true;

            return false;
        }
    }
}
