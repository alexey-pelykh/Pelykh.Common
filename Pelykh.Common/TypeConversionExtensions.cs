using System;
using System.ComponentModel;

namespace Pelykh.Common
{
    public static class TypeConversionExtensions
    {
        public static bool CanConvertFromString(this Type type)
        {
            type.ThrowIfNull("this");

            return type.IsSimpleUnderlyingType() || HasStringConverter(type);
        }

        public static bool HasStringConverter(this Type type)
        {
            type.ThrowIfNull("this");

            return TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));
        }
    }
}
