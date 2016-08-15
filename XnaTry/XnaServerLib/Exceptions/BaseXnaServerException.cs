using System;

namespace XnaServerLib.Exceptions
{
    public class BaseXnaServerException : Exception
    {
        public DateTime TimeStamp { get; }

        public BaseXnaServerException()
        {
            TimeStamp = DateTime.Now;
        }

        public BaseXnaServerException(string message) 
            : base(message)
        {
            TimeStamp = DateTime.Now;
        }

        public BaseXnaServerException(string message, Exception innerException) 
            : base(message, innerException)
        {
            TimeStamp = DateTime.Now;
        }
    }
}