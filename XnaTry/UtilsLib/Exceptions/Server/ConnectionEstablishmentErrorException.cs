using System;

namespace UtilsLib.Exceptions.Server
{
    public class ConnectionEstablishmentErrorException : BaseXnaServerException
    {
        public ConnectionEstablishmentErrorException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}