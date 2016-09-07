using System.IO;
using System.Net.Sockets;

namespace XnaCommonLib.Network
{
    public static class HelperMethods
    {
        public static bool Receive(TcpClient connection, BinaryReader reader, PacketProtocol packetProtocol)
        {
            var bufferSize = connection.Available;
            if (bufferSize <= 0)
                return false;

            var buffer = reader.ReadBytes(bufferSize);
            packetProtocol.DataReceived(buffer);
            return true;
        }
    }
}
