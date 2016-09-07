using System;

namespace UtilsLib.Exceptions.Common
{
    public class BaseGameException : Exception
    {
        public DateTime TimeStamp { get; }

        public BaseGameException()
        {
            TimeStamp = DateTime.Now;
        }

        public BaseGameException(string message) 
            : base(message)
        {
            TimeStamp = DateTime.Now;
        }

        public BaseGameException(string message, Exception innerException) 
            : base(message, innerException)
        {
            TimeStamp = DateTime.Now;
        }
    }
}