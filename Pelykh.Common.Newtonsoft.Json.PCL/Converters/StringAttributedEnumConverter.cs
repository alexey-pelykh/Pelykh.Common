using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;

namespace Pelykh.Common.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="Enum"/> to and from its string value specified by <see cref="JsonEnumValueAttribute"/>.
    /// </summary>
    public class StringAttributedEnumConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var objectType = value.GetType();
            var underlyingObjectType = objectType.GetNotNullableTypeOrOriginal();

            if (!underlyingObjectType.IsEnum)
            {
                throw new JsonSerializationException(string.Format(
                    "Cannot convert {0} to enum value",
                    value));
            }

            var enumValueDeclaration = underlyingObjectType.GetField(value.ToString());
            var enumValueAttribute = (JsonEnumValueAttribute)enumValueDeclaration
                .GetCustomAttributes(typeof(JsonEnumValueAttribute), false)
                .FirstOrDefault();

            if (enumValueAttribute == null)
            {
                throw new JsonSerializationException(string.Format(
                    "Cannot convert {0} to enum value",
                    value));
            }

            writer.WriteValue(enumValueAttribute.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var underlyingObjectType = objectType.GetNotNullableTypeOrOriginal();
            var objectTypeIsNullable = (underlyingObjectType != objectType);

            if (!underlyingObjectType.IsEnum)
            {
                throw new JsonSerializationException(string.Format(
                    "Cannot convert value to non-enum {0}",
                    objectType));
            }

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
            
            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException(string.Format(
                    "Cannot convert {0} token to {1}",
                    reader.TokenType.ToString(),
                    objectType));
            }

            try
            {
                var jsonValue = reader.Value.ToString();

                foreach (var enumValueDeclaration in underlyingObjectType.GetFields(BindingFlags.Public|BindingFlags.Static))
                {
                    var enumValueAttribute = (JsonEnumValueAttribute)enumValueDeclaration
                        .GetCustomAttributes(typeof(JsonEnumValueAttribute), false)
                        .FirstOrDefault();

                    if (enumValueAttribute == null)
                        continue;

                    if (enumValueAttribute.Value == jsonValue)
                        return enumValueDeclaration.GetValue(null);
                }
            }
            catch (Exception e)
            {
                throw new JsonSerializationException(string.Format(
                    "Error parsing value {0} as {1}",
                    reader.Value,
                    objectType), e);
            }

            return existingValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.GetNotNullableTypeOrOriginal().IsEnum;
        }
    }
}
