using XnaServerLib;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.Listen();
        }
    }
}
