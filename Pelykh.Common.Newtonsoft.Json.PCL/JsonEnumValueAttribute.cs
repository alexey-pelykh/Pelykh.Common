using System;

namespace Pelykh.Common.Newtonsoft.Json
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class JsonEnumValueAttribute : Attribute
    {
        public string Value { get; private set; }

        public JsonEnumValueAttribute(string value)
        {
            Value = value;
        }
    }
}
