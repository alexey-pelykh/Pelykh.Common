using System;

namespace Pelykh.Common
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            value.ThrowIfNull("this");

            if (value.Length == 0)
                return value;

            if (value.Length == 1)
                return value.ToLower();

            return Char.ToLower(value[0]) + value.Substring(1);
        }
    }
}
