using System;

namespace UtilsLib.Exceptions.Server.Commands
{
    public class ServerCommandsException : Exception
    {
        public ServerCommandsException(string message) 
            : base(message)
        {
        }

        public ServerCommandsException()
        {
        }

        public ServerCommandsException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}