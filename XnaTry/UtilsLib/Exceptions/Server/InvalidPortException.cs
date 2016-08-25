using System.Net;

namespace UtilsLib.Exceptions.Server
{
    public class InvalidPortException : BaseXnaServerException
    {
        public InvalidPortException(int port) 
            : base(ErrorMessage(port))
        {
        }

        private static string ErrorMessage(int port)
        {
            return string.Format("Attempt to connect to a server, supplying an invalid port ({0}). Port should be between {1} and {2}", port, IPEndPoint.MinPort,
                IPEndPoint.MaxPort);
        }
    }
}
