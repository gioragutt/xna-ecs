using System;

namespace XnaServerLib.Exceptions
{
    public class ServerAlreadyRunningException : BaseXnaServerException
    {
        private const string ErrorMessage = "Attemp to call Listen() when server is already listening";

        public ServerAlreadyRunningException() 
            : base(ErrorMessage)
        {
        }
    }
}
