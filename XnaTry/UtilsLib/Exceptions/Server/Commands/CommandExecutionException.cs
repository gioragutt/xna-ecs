using System;

namespace UtilsLib.Exceptions.Server.Commands
{
    public class CommandExecutionException : ServerCommandsException
    {
        public CommandExecutionException(string message) 
            : base(message)
        {
        }

        public CommandExecutionException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
