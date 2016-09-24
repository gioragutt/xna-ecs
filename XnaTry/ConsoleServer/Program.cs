using System;
using EMS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XnaServerLib;

namespace ConsoleServer
{
    public class ServerMessagesPrinter : EmsClient, IDisposable
    {
        public ServerMessagesPrinter()
        {
            SubscribeToAll(Callback_ToAll);
        }

        private static void Callback_ToAll(JObject message)
        {
            Console.WriteLine("Received message: {0}", message.ToString(Formatting.Indented));
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
            server.StartListen(false);
        }
    }
}
