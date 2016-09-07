using System;

namespace UtilsLib.Consts
{
    public static partial class Constants
    {
        public static class Time
        {
            public const float MillisecondsInSecond = 1000f;
            public const int UpdatesPerSecond = 60;
            public const int UpdateThreadSleepTime = (int)MillisecondsInSecond / UpdatesPerSecond;
            public static readonly TimeSpan MaxTimeout = TimeSpan.FromMilliseconds(5000);
        }
    }
}
