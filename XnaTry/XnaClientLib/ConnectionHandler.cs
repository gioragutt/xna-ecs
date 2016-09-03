using ECS.BaseTypes;
using EMS;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UtilsLib;
using UtilsLib.Consts;
using XnaClientLib.ECS;
using XnaClientLib.ECS.Compnents.Network;
using XnaCommonLib.ECS;
using XnaCommonLib.Network;

namespace XnaClientLib
{
    public class ConnectionHandler : EmsClient, IDisposable
    {
        public void Dispose()
        {
            if (Connected)
                Connection.Close();
        }

        #region Network Components and Properties

        /// <summary>
        /// The EMS Server Endpoint to receive and broadcasts from and to the server
        /// </summary>
        private EmsServerEndpoint EmsServerEndpoint { get; }

        /// <summary>
        /// The TcpClient object with which connection is established
        /// </summary>
        protected TcpClient Connection { get; set; }

        /// <summary>
        /// Used for reading data sent from the client
        /// </summary>
        public BinaryReader Reader { get; protected set; }

        /// <summary>
        /// Used to send data to the client
        /// </summary>
        public BinaryWriter Writer { get; protected set; }

        /// <summary>
        /// The servers port
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// The servers host name
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Indicates whether the client is connected to the server or not
        /// </summary>
        public bool Connected
        {
            get
            {
                return Connection != null && Connection.Connected;
            }
        }

        /// <summary>
        /// Stores the time of the last update from the server
        /// </summary>
        public DateTime LastUpdateTime { get; private set; }

        /// <summary>
        /// Stores the time between the last two update
        /// </summary>
        public TimeSpan LastPing { get; private set; }

        #endregion

        #region Game Components and Properties

        /// <summary>
        /// The thread on which the player updates are received and sent
        /// </summary>
        private Thread UpdateThread { get; }

        /// <summary>
        /// The game object of the player
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// The ClientGameManager instance of the game
        /// </summary>
        public ClientGameManager ClientGameManager { get; }

        #endregion

        #region Constructor

        public ConnectionHandler(string hostName, int port, ClientGameManager gameManager)
        {
            HostName = hostName;
            Port = port;
            ClientGameManager = gameManager;
            GameObject = null;
            UpdateThread = new Thread(ConnectionHandler_InteractWithServer);
            EmsServerEndpoint = new EmsServerEndpoint();
        }

        #endregion

        #region API

        public void ConnectAndInitializeLocalPlayer(string name, string team)
        {
            if (Connection != null && Connection.Connected)
                throw new InvalidOperationException("Attempt to connect when already connected");

            var hostname = string.IsNullOrEmpty(HostName) ? "localhost" : HostName;

            Connection = new TcpClient(hostname, Port);

            Reader = new BinaryReader(Connection.GetStream());
            Writer = new BinaryWriter(Connection.GetStream());

            WriteLoginDataToServer(name, team);
            ReadLoginResponseFromServer();
            WritePlayerData();
            UpdateThread.Start();
        }

        #endregion

        #region Communication With Server Methods

        private void ReadLoginResponseFromServer()
        {
            var response = Reader.ReadString();
            LastUpdateTime = DateTime.Now;
            var playerUpdate = JsonConvert.DeserializeObject<PlayerUpdate>(response);
            GameObject = ClientGameManager.BeginAllocateLocal(playerUpdate.Guid);
            GameObject.Components.Get<NetworkPlayer>().Update(playerUpdate);
            ClientGameManager.EndAllocate(GameObject);
        }

        private void WriteLoginDataToServer(string name, string team)
        {
            var loginMessage = new ClientLoginMessage
            {
                PlayerName = name,
                PlayerTeam = team
            };
            var serializedMessage = JsonConvert.SerializeObject(loginMessage);
            Writer.Write(serializedMessage);
        }

        private void ConnectionHandler_InteractWithServer()
        {
            while (Connection.Connected)
            {
                ProcessServerUpdate();
                WritePlayerData();
                Thread.Sleep(Constants.Time.UpdateThreadSleepTime);
            }
        }

        private void ProcessServerUpdate()
        {
            var message = Reader.ReadString();
            UpdatePing();

            var incomingUpdate = JsonConvert.DeserializeObject<ServerToClientUpdateMessage>(message);
            EmsServerEndpoint.BroadcastIncomingEvents(incomingUpdate.Broadcasts);

            foreach (var update in incomingUpdate.PlayerUpdates)
                ApplyUpdate(update);
        }

        private void ApplyUpdate(PlayerUpdate update)
        {
            var entity = new Entity(update.Guid);
            if (!ClientGameManager.EntityPool.Exists(entity))
            {
                var newGo = ClientGameManager.BeginAllocateRemote(entity.Id);
                newGo.Components.Get<NetworkPlayer>().Update(update);
                ClientGameManager.EndAllocate(newGo);
            }
            else
            {
                var remoteComponents = ClientGameManager.EntityPool.GetComponents(entity);
                remoteComponents.Get<NetworkPlayer>().Update(update);
            }
        }

        private void WritePlayerData()
        {
            var message = new ClientToServerUpdateMessage
            {
                Broadcasts = EmsServerEndpoint.Flush(),
                PlayerUpdate = new PlayerUpdate(GameObject.Components)
            };
            Writer.Write(JsonConvert.SerializeObject(message));
        }

        #endregion

        #region Helper Methods

        private void UpdatePing()
        {
            LastPing = DateTime.Now - LastUpdateTime;
            LastUpdateTime = DateTime.Now;
        }

        #endregion
    }
}
