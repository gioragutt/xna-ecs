using System;
using System.Diagnostics;
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

        public void Update(TimeSpan elapsedTimeout)
        {
            Elapsed = elapsedTimeout;

            if (Elapsed >= MaxTime && !Debugger.IsAttached)
                throw new CommunicationTimeoutException(MaxTime, Elapsed, BeginningOfTimeoutSampling);
        }

        public void Reset()
        {
            Elapsed = TimeSpan.Zero;
            BeginningOfTimeoutSampling = DateTime.Now;
        }
    }
}
