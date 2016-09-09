using EMS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UtilsLib;
using UtilsLib.Consts;
using UtilsLib.Exceptions.Server;
using XnaCommonLib.ECS.Components;
using XnaServerLib.ECS;
using XnaServerLib.ECS.Systems;

namespace XnaServerLib
{
    public class Server : EmsClient
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

        public MapManager MapManager { get; }

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

            MapManager = new MapManager("xna_try_map1.tmx");

            ConnectionListener = new TcpListener(IPAddress.Any, Port);
            Listening = false;

            GameManager = new ServerGameManager(this);
            GameManager.RegisterSystem(new MovementSystem());

            UpdateLoopThread.Start();

            Subscribe(Constants.Messages.DamagePlayers, Callback_DamagePlayers);
        }

        private void Callback_DamagePlayers(JObject message)
        {
            var player = message.GetGuid(Constants.Fields.PlayerGuid);
            var damage = message.GetProp("damage", 10f);

            var attrs = GameManager.EntityPool.GetAllOf<PlayerAttributes>().ToList();
            if (player != null)
                attrs = attrs.Where(c => c.Container.Parent.Id == player).ToList();

            attrs.ForEach(a => a.Health -= damage);
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

                Thread.Sleep(Constants.Time.UpdateThreadSleepTime);
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
                        Console.WriteLine("{0} Accepted new connection", DateTime.Now.TimeOfDay);
                        var newGameClient = new GameClient(acceptedConnection, GameManager.CreateGameObject(), GameManager);
                        var attr = newGameClient.GameObject.Components.Get<PlayerAttributes>();
                        Console.WriteLine("{2} - {0} Connected to {1}", attr.Name, attr.Team.Name, acceptedConnection.Client.RemoteEndPoint);
                        GameClients.Add(newGameClient);
                        Broadcast(
                            MessageBuilder.Create(Constants.Messages.ClientAcceptedOnServer)
                                .Add(Constants.Fields.PlayerName, attr.Name)
                                .Add(Constants.Fields.TeamName, attr.Team.Name)
                                .Get());
                }
                catch (SocketException se)
                {
                    Console.WriteLine(new ConnectionEstablishmentErrorException("Encountered a network error while trying to accept a new client", se));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(new ConnectionEstablishmentErrorException("Encountered an error while trying to accept a new client", ex));
                }
            }
        }

        #endregion Thread Methods
    }
}
