using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Pelykh.Common.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Performs default conversion. If conversion failed, default value is used.
    /// </summary>
    public sealed class DefaultValueFallbackConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            try
            {
                JsonConvert.SerializeObject(Activator.CreateInstance(objectType));
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return JToken.ReadFrom(reader).ToObject(objectType, serializer);
            }
            catch (JsonSerializationException)
            {
                return existingValue;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken.FromObject(value, serializer).WriteTo(writer);
        }
    }
}
