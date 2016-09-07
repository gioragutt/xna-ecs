using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsLib.Exceptions.Common
{
    public class CommunicationTimeoutException : BaseGameException
    {
        public TimeSpan MaxTimeout { get; }
        public TimeSpan ElapsedReached { get; }
        public DateTime BeginningOfTimeoutSample { get; }
        public TimeSpan LastDelta { get; }

        public CommunicationTimeoutException(
            TimeSpan maxTimeout,
            TimeSpan elapsedReached,
            DateTime beginningOfTimeoutSample, TimeSpan lastDelta) 
            : this(null, null, maxTimeout, elapsedReached, beginningOfTimeoutSample, lastDelta)
        {
        }

        public CommunicationTimeoutException(string message, CommunicationTimeoutException other)
            : this(message, other.MaxTimeout, other.ElapsedReached, other.BeginningOfTimeoutSample, other.LastDelta)
        {

        }

        public CommunicationTimeoutException(
            string message,
            Exception innerException,
            TimeSpan maxTimeout,
            TimeSpan elapsedReached,
            DateTime beginningOfTimeoutSample, TimeSpan lastDelta) : base(message, innerException)
        {
            MaxTimeout = maxTimeout;
            ElapsedReached = elapsedReached;
            BeginningOfTimeoutSample = beginningOfTimeoutSample;
            LastDelta = lastDelta;
        }

        public CommunicationTimeoutException(
            string message,
            TimeSpan maxTimeout,
            TimeSpan elapsedReached,
            DateTime beginningOfTimeoutSample, TimeSpan lastDelta)
            : this(message, null, maxTimeout, elapsedReached, beginningOfTimeoutSample, lastDelta)
        {
        }

        public override string ToString()
        {
            return
                string.Format(
                    "MaxTimeout: {0}\nElapsedReached: {1}\nBeginningOfTimeoutSamping: {2}\nTimeStamp: {3}\n LastDelta: {4}",
                    MaxTimeout, 
                    ElapsedReached, 
                    BeginningOfTimeoutSample.TimeOfDay, 
                    TimeStamp.TimeOfDay, 
                    LastDelta);
        }
    }
}
