using System;
using System.Text;
using EMS;
using XnaCommonLib;
using XnaServerLib;

namespace ConsoleServer
{
    public class ServerMessagesPrinter : EmsClient, IDisposable
    {
        public ServerMessagesPrinter()
        {
            SubscribeToAll(Callback_ToAll);
        }

        private static void Callback_ToAll(EventMessageData eventMessageData)
        {
            Console.WriteLine("Received message: {0}", eventMessageData.Name);
        }

        public void Dispose()
        {
            UnsubscribeFromAll();
        }
    }
    class Program
    {
        private static void Main(string[] args)
        {
            var printer = new ServerMessagesPrinter();
            var server = new Server();
            server.Listen();
        }
    }
}
