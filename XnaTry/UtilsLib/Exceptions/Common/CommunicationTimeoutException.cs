﻿using System;

namespace UtilsLib.Exceptions.Common
{
    public class CommunicationTimeoutException : BaseGameException
    {
        public TimeSpan MaxTimeout { get; }
        public TimeSpan ElapsedReached { get; }
        public DateTime BeginningOfTimeoutSample { get; }

        public CommunicationTimeoutException(
            TimeSpan maxTimeout,
            TimeSpan elapsedReached,
            DateTime beginningOfTimeoutSample) : this(null, null, maxTimeout, elapsedReached, beginningOfTimeoutSample)
        {
        }

        public CommunicationTimeoutException(string message, CommunicationTimeoutException other)
            : this(message, other.MaxTimeout, other.ElapsedReached, other.BeginningOfTimeoutSample)
        {
        }

        public CommunicationTimeoutException(
            string message,
            Exception innerException,
            TimeSpan maxTimeout,
            TimeSpan elapsedReached,
            DateTime beginningOfTimeoutSample) : base(message, innerException)
        {
            MaxTimeout = maxTimeout;
            ElapsedReached = elapsedReached;
            BeginningOfTimeoutSample = beginningOfTimeoutSample;
        }

        public CommunicationTimeoutException(
            string message,
            TimeSpan maxTimeout,
            TimeSpan elapsedReached,
            DateTime beginningOfTimeoutSample)
            : this(message, null, maxTimeout, elapsedReached, beginningOfTimeoutSample)
        {
        }

        public override string ToString()
        {
            return
                string.Format(
                    "MaxTimeout: {0}\nElapsedReached: {1}\nBeginningOfTimeoutSamping: {2}\nTimeStamp: {3}\n",
                    MaxTimeout, 
                    ElapsedReached, 
                    BeginningOfTimeoutSample.TimeOfDay, 
                    TimeStamp.TimeOfDay);
        }
    }
}
