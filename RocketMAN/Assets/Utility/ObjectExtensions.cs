using System;

namespace Utility
{
    public static class ObjectExtensions
    {
        public static T Cast<T>(this object obj)
        {
            return (T) obj;
        }

        public static T ThrowIfNull<T>(this T obj, Func<Exception> exceptionFactory)
        {
            if (obj is null)
            {
                throw exceptionFactory();
            }

            return obj;
        }
    }
}