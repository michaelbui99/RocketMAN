using System;
using Unity.VisualScripting;

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

        public static T InvokeActionIfNotNull<T>(this T obj, Action action)
        {
            if (obj is null)
            {
                return obj;
            }
            
            action();
            return obj;
        }
    }
}