using System.Net;
using XnaServerLib.Exceptions;

namespace XnaServerLib
{
    public static class ServerUtil
    {
        public static void AssertPortIsValid(int port)
        {
            if (IsPortOutOfBounds(port))
                throw new InvalidPortException(port);
        }

        private static bool IsPortOutOfBounds(int port)
        {
            return port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort;
        }
    }
}
