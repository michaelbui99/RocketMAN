using System;

namespace Utility
{
    public class SwitchUtil
    {
        public static void Unreachable()
        {
            throw new UnreachableExpressionReachedException();
        }

        public static T Unreachable<T>()
        {
            throw new UnreachableExpressionReachedException();
        }
    }

    public class UnreachableExpressionReachedException : Exception
    {
        public UnreachableExpressionReachedException()
        {
        }

        public UnreachableExpressionReachedException(string msg) : base(msg)
        {
        }
    }
}