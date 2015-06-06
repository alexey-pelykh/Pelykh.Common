using System;

namespace Pelykh.Common
{
    public static class TypeExtensions
    {
        public static bool IsSimpleType(this Type type)
        {
            type.ThrowIfNull("this");

            return
                type.IsPrimitive ||
                type.Equals(typeof(string)) ||
                type.Equals(typeof(DateTime)) ||
                type.Equals(typeof(Decimal)) ||
                type.Equals(typeof(Guid)) ||
                type.Equals(typeof(DateTimeOffset)) ||
                type.Equals(typeof(TimeSpan));
        }

        public static bool IsSimpleUnderlyingType(this Type type)
        {
            type.ThrowIfNull("this");

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
                type = underlyingType;

            return IsSimpleType(type);
        }

        public static bool IsConstructible(this Type type, params Type[] constructorArguments)
        {
            type.ThrowIfNull("this");

            if (type.IsValueType)
                return true;

            return
                !type.IsAbstract &&
                type.GetConstructor(constructorArguments) != null;
        }

        public static bool IsNullable(this Type type)
        {
            return
                type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type GetNotNullableType(this Type type)
        {
            return type.IsNullable()
                ? Nullable.GetUnderlyingType(type)
                : null;
        }

        public static Type GetNotNullableTypeOrOriginal(this Type type)
        {
            return type.IsNullable()
                ? Nullable.GetUnderlyingType(type)
                : type;
        }
    }
}
