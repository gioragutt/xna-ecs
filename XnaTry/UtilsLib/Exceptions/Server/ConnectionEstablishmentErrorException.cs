using System;
using UtilsLib.Exceptions.Common;

namespace UtilsLib.Exceptions.Server
{
    public class ConnectionEstablishmentErrorException : BaseGameException
    {
        public ConnectionEstablishmentErrorException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}