using System;
using System.ComponentModel;
using System.Globalization;

namespace Pelykh.Common
{
    public static class TypeConversionHelpers
    {
        public static object ConvertSimpleType(CultureInfo culture, object value, Type destinationType)
        {
            culture.ThrowIfNull("culture");
            value.ThrowIfNull("value");
            destinationType.ThrowIfNull("destinationType");

            if (value == null || destinationType.IsInstanceOfType(value))
                return value;

            var converter = TypeDescriptor.GetConverter(destinationType);
            bool canConvertFrom = converter.CanConvertFrom(value.GetType());
            if (!canConvertFrom)
                converter = TypeDescriptor.GetConverter(value.GetType());
            if (!(canConvertFrom || converter.CanConvertTo(destinationType)))
            {
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);

                Type underlyingType = Nullable.GetUnderlyingType(destinationType);
                if (underlyingType != null)
                    return ConvertSimpleType(culture, value, underlyingType);

                throw new InvalidOperationException(
                    string.Format("Cannot convert '{0}' to '{1}'", value.GetType(), destinationType));
            }

            try
            {
                return canConvertFrom
                           ? converter.ConvertFrom(null, culture, value)
                           : converter.ConvertTo(null, culture, value, destinationType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot convert '{0}' to '{1}'", value.GetType(), destinationType),
                    ex);
            }
        }
    }
}
