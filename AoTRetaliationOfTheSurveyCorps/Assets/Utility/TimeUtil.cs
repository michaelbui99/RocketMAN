using System;

namespace Utility
{
    public static class TimeUtil
    {
        public static float GetTimeDifferenceInSeconds(DateTime start, DateTime end)
        {
            return (float) (end - start).TotalSeconds;
        }
    }
}