using System.Collections.Generic;

namespace UtilsLib.Exceptions.Server.Commands
{
    public class InvalidCommandParametersException : ServerCommandsException
    {
        public IList<string> Parameters { get; }

        public InvalidCommandParametersException(IList<string> parameters)
        {
            Parameters = parameters;
        }
    }
}