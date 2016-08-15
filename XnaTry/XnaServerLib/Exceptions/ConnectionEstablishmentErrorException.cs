using System;

namespace XnaServerLib.Exceptions
{
    public class ConnectionEstablishmentErrorException : BaseXnaServerException
    {
        public ConnectionEstablishmentErrorException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}