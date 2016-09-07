using System;
using UtilsLib.Exceptions.Common;

namespace XnaCommonLib.Network
{
    public class TimeoutTimer
    {
        public TimeSpan MaxTime { get; }
        public TimeSpan Elapsed { get; private set; }
        public DateTime BeginningOfTimeoutSampling { get; private set; }

        public TimeoutTimer(TimeSpan maxTime)
        {
            Reset();           
            MaxTime = maxTime;
        }

        public void Update(TimeSpan delta)
        {
            Elapsed = delta;

            if (Elapsed < MaxTime)
                return;

            throw new CommunicationTimeoutException(MaxTime, Elapsed, BeginningOfTimeoutSampling, delta);
        }

        public void Reset()
        {
            Elapsed = TimeSpan.Zero;
            BeginningOfTimeoutSampling = DateTime.Now;
        }
    }
}
