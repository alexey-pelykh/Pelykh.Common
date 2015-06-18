using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pelykh.Common.Newtonsoft.Json
{
    public static class JsonEnumValue
    {
        public static string Get(object input)
        {
            input.ThrowIfNull("input");

            var type = input.GetType();
            if (!type.IsEnum)
                throw new ArgumentException(string.Format("{0} is not enum", type), "input");

            var enumValueDeclaration = type.GetField(input.ToString());
            var enumValueAttribute = (JsonEnumValueAttribute)enumValueDeclaration
                .GetCustomAttributes(typeof(JsonEnumValueAttribute), false)
                .FirstOrDefault();

            if (enumValueAttribute == null)
            {
                throw new ArgumentException(string.Format(
                    "{0} has no {1} attribute",
                    input,
                    typeof(JsonEnumValueAttribute)));
            }

            return enumValueAttribute.Value;
        }
    }
}
