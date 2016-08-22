using System;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;
using XnaServerLib.ECS;
using XnaServerLib.ECS.Systems;
using XnaServerLib.Exceptions;

namespace XnaServerLib
{
    public class Server
    {
        #region Constants

        /// <summary>
        /// The default port on which the server will run
        /// </summary>
        public const int DefaultPort = 27015;

        #endregion Constants

        #region Threads

        /// <summary>
        /// The thread that accepts clients to the server
        /// </summary>
        private Thread ClientAcceptingThread { get; }

        /// <summary>
        /// The thread that runs the update loop
        /// </summary>
        private Thread UpdateLoopThread { get; }

        #endregion Threads

        #region Properties

        /// <summary>
        /// The list of client currently registered in the server
        /// </summary>
        public List<GameClient> GameClients { get; set; }

        /// <summary>
        /// TcpListner which listens for incoming TCP/IP Connections
        /// </summary>
        private TcpListener ConnectionListener { get; }

        /// <summary>
        /// The port on which the server runs
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Indicates whether the server is listening for new connections
        /// </summary>
        public bool Listening { get; private set; }

        /// <summary>
        /// The servers GameManager
        /// </summary>
        public ServerGameManager GameManager { get; }

        public DateTime LastUpdateTime { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes the server
        /// </summary>
        /// <param name="port"></param>
        public Server(int port = DefaultPort)
        {
            Console.WriteLine("Initializing server");
            GameClients = new List<GameClient>();

            Port = port;
            ClientAcceptingThread = new Thread(Server_AcceptPlayers)
            {
                IsBackground = false
            };

            UpdateLoopThread = new Thread(Server_UpdateLoop)
            {
                IsBackground = true
            };

            ConnectionListener = new TcpListener(IPAddress.Any, Port);
            Listening = false;

            GameManager = new ServerGameManager();
            GameManager.RegisterSystem(new MovementSystem());

            UpdateLoopThread.Start();
        }

        #endregion Constructors

        #region API Methods

        public void Listen()
        {
            if (Listening)
                throw new ServerAlreadyRunningException();

            Console.WriteLine("Server listening");
            ClientAcceptingThread.Start();
            Listening = true;
        }

        #endregion API Methods

        #region Thread Methods

        private void Server_UpdateLoop()
        {
            while (true)
            {
                var currentTime = DateTime.Now;
                GameManager.Update(currentTime - LastUpdateTime);
                LastUpdateTime = currentTime;

                Thread.Sleep(Constants.UpdateThreadSleepTime);
            }
        }

        private void Server_AcceptPlayers()
        {
            ConnectionListener.Start();

            while (Listening)
            {
                try
                {
                    var acceptedConnection = ConnectionListener.AcceptTcpClient();
                    Console.WriteLine("Accepted new connection");
                    var newGameClient = new GameClient(acceptedConnection, GameManager.CreateGameObject(), GameManager);
                    var attr = newGameClient.GameObject.Components.Get<PlayerAttributes>();
                    Console.WriteLine("{2} - {0} Connected to {1}", attr.Name, attr.Team.Name, acceptedConnection.Client.RemoteEndPoint);
                    GameClients.Add(newGameClient);
                }
                catch (SocketException se)
                {
                    throw new ConnectionEstablishmentErrorException("Encountered a network error while trying to accept a new client", se);
                }
                catch (Exception ex)
                {
                    throw new ConnectionEstablishmentErrorException("Encountered an error while trying to accept a new client", ex);
                }
            }
        }

        #endregion Thread Methods
    }
}
