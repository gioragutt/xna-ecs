using System.Net;
using UtilsLib.Exceptions.Server;

namespace UtilsLib.Utility
{

    public static partial class Utils
    {
        public static class Server
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
}
