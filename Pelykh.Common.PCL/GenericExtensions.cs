using System;

namespace Pelykh.Common
{
    public static class GenericExtensions
    {
        public static void ThrowIfNull<T>(this T obj, string paramName = null)
            where T : class
        {
            if (obj != null)
                return;

            if (paramName != null)
                throw new ArgumentNullException(paramName);

            throw new ArgumentNullException();
        }

        public static void ThrowIfNotAssignableTo<TParent>(this Type type, string paramName = null)
        {
            type.ThrowIfNull("type");

            if (typeof(TParent).IsAssignableFrom(type))
                return;

            throw new ArgumentException(
                string.Format("{0} must be assignable to {1}", type, typeof(TParent)),
                paramName);
        }
    }
}
