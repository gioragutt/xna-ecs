using EMS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UtilsLib;
using UtilsLib.Consts;
using UtilsLib.Exceptions.Server;
using XnaCommonLib.ECS.Components;
using XnaServerLib.Commands;
using XnaServerLib.Commands.BaseClasses;
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

        #region Events

        public event EventHandler<PlayerObjectEventArgs> ClientConnected;
        public event EventHandler<PlayerObjectEventArgs> ClientUpdateReceived;
        public event EventHandler<PlayerIdEventArgs> ClientDisconnected;

        #endregion

        #region Threads

        /// <summary>
        /// The thread that accepts clients to the server
        /// </summary>
        private Thread clientAcceptingThread;

        /// <summary>
        /// The thread that runs the update loop
        /// </summary>
        private Thread updateLoopThread;

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
        public int Port { get; set; }

        /// <summary>
        /// Indicates whether the server is listening for new connections
        /// </summary>
        public bool Listening { get; private set; }

        /// <summary>
        /// Indicates whether the server is listening for new connections
        /// </summary>
        private bool threadsRunning;

        /// <summary>
        /// The servers GameManager
        /// </summary>
        public ServerGameManager GameManager { get; }

        /// <summary>
        /// The time the last update occured
        /// </summary>
        public DateTime LastUpdateTime { get; private set; }

        /// <summary>
        /// Manages loading and interacting with the map
        /// </summary>
        public MapManager MapManager { get; }

        public ServerCommandsService CommandsService { get; private set; }

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

            MapManager = new MapManager("xna_try_map1.tmx");
            CommandsService = new ServerCommandsService(this);
            ConnectionListener = new TcpListener(IPAddress.Any, Port);
            Listening = false;
            threadsRunning = false;
            GameManager = new ServerGameManager(this);
            GameManager.RegisterSystem(new MovementSystem());

            Subscribe(Constants.Messages.DamagePlayers, Callback_DamagePlayers);
        }

        #endregion Constructors

        #region Callbacks

        private void Callback_DamagePlayers(JObject message)
        {
            var player = message.GetProp<string>(Constants.Fields.PlayerName);
            var damage = message.GetProp("damage", 10f).ToString(CultureInfo.InvariantCulture);
            if (player == null)
                CommandsService.Execute("damage", damage);
            else
                CommandsService.Execute("damage", damage, player);
        }

        #endregion

        #region API Methods

        public void StartListen(bool isBackground = true)
        {
            if (threadsRunning)
                throw new ServerAlreadyRunningException();

            Console.WriteLine("StartListen");
            threadsRunning = true;
            StartAcceptingClients(isBackground);
            StartUpdatingServer(isBackground);
        }

        public void StopListen()
        {
            Console.WriteLine("StopListen");
            threadsRunning = false;
            ConnectionListener.Stop();
            var clients = GameClients.Count;
            for (var i = 0; i < clients; ++i)
            {
                GameClients[0].StopClient();
                GameClients.RemoveAt(0);
            }
        }

        #endregion API Methods

        #region Thread Methods

        #region Update Loop Thread

        private void Server_UpdateLoop()
        {
            while (threadsRunning)
            {
                var currentTime = DateTime.Now;
                GameManager.Update(currentTime - LastUpdateTime);
                LastUpdateTime = currentTime;

                Thread.Sleep(Constants.Time.UpdateThreadSleepTime);
            }
        }

        private void StartUpdatingServer(bool isBackground)
        {
            updateLoopThread = null;
            updateLoopThread = new Thread(Server_UpdateLoop)
            {
                IsBackground = isBackground,
                Name = "Update Server Thread"
            };

            updateLoopThread.Start();
        }

        #endregion Update Loop Thread

        #region Client Accepting Thread

        private void Server_AcceptPlayers()
        {
            ConnectionListener.Start();
            Listening = true;

            while (threadsRunning)
            {
                try
                {
                    var acceptedConnection = ConnectionListener.AcceptTcpClient();
                    Console.WriteLine("{0} Accepted new connection", DateTime.Now.TimeOfDay);
                    var newGameClient = new GameClient(acceptedConnection, GameManager.CreateGameObject(), GameManager);
                    var attr = newGameClient.GameObject.Components.Get<PlayerAttributes>();
                    Console.WriteLine("{2} - {0} Connected to {1}", attr.Name, attr.Team.Name,
                        acceptedConnection.Client.RemoteEndPoint);
                    GameClients.Add(newGameClient);
                    Broadcast(
                        MessageBuilder.Create(Constants.Messages.ClientAcceptedOnServer).Add(
                            Constants.Fields.PlayerName, attr.Name).Add(Constants.Fields.TeamName, attr.Team.Name).Get());

                    OnClientConnected(new PlayerObjectEventArgs
                    {
                        GameObject = newGameClient.GameObject
                    });
                }
                catch (ThreadAbortException)
                {
                    break;
                }
                catch (SocketException se)
                {
                    Console.WriteLine("Encountered a network error while trying to accept a new client: " + se.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Encountered an error while trying to accept a new client: " + ex.Message);
                }
            }

            Listening = false;
            if (ConnectionListener.Server.Connected)
                ConnectionListener.Stop();
        }

        private void StartAcceptingClients(bool isBackground)
        {
            clientAcceptingThread = null;
            clientAcceptingThread = new Thread(Server_AcceptPlayers)
            {
                IsBackground = isBackground,
                Name = "Client Accepting Thread"
            };
            clientAcceptingThread.Start();
        }

        #endregion

        #endregion Thread Methods
        
        #region Event Invoketions

        protected virtual void OnClientConnected(PlayerObjectEventArgs e)
        {
            ClientConnected?.Invoke(this, e);
        }
        public virtual void OnClientDisconnected(PlayerIdEventArgs e)
        {
            ClientDisconnected?.Invoke(this, e);
        }

        public virtual void OnClientUpdateReceived(PlayerObjectEventArgs e)
        {
            ClientUpdateReceived?.Invoke(this, e);
        }

        #endregion
    }
}
