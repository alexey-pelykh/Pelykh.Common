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
    }
}
