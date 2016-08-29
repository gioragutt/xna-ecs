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
using XnaClientLib.ECS.Compnents;
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
        /// The thread on which the player updates are received and sent
        /// </summary>
        private Thread UpdateThread { get; }

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
        /// The game object of the player
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// The ClientGameManager instance of the game
        /// </summary>
        public ClientGameManager ClientGameManager { get; }

        /// <summary>
        /// The EMS Server Endpoint to receive and broadcasts from and to the server
        /// </summary>
        private EmsServerEndpoint EmsServerEndpoint { get; }

        public ConnectionHandler(string hostName, int port, ClientGameManager gameManager)
        {
            HostName = hostName;
            Port = port;
            ClientGameManager = gameManager;
            GameObject = null;
            UpdateThread = new Thread(ConnectionHandler_InteractWithServer);
            EmsServerEndpoint = new EmsServerEndpoint();
        }

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

        private void ReadLoginResponseFromServer()
        {
            var response = Reader.ReadString();
            var playerUpdate = JsonConvert.DeserializeObject<PlayerUpdate>(response);
            GameObject = ClientGameManager.BeginAllocateLocal(playerUpdate.Guid);
            GameObject.Components.Get<NetworkPlayer>().Update(playerUpdate);
            ClientGameManager.EndAllocate(GameObject);
        }

        private void WriteLoginDataToServer(string name, string team)
        {
            var loginMessage = MessageBuilder.Create(Constants.Messages.PlayerLogin)
                .Add(Constants.Fields.PlayerName, name)
                .Add(Constants.Fields.TeamName, team)
                .Get();
            var serializedMessage = JsonConvert.SerializeObject(loginMessage);
            Writer.Write(serializedMessage);
        }

        private void ConnectionHandler_InteractWithServer()
        {
            while (Connection.Connected)
            {
                var message = Reader.ReadString();
                var incomingUpdate = JsonConvert.DeserializeObject<IncomingUpdate>(message);
                EmsServerEndpoint.BroadcastIncomingEvents(incomingUpdate.Broadcasts);

                foreach (var update in incomingUpdate.PlayerUpdates)
                    ApplyUpdate(update);

                WritePlayerData();
                Thread.Sleep(Constants.Time.UpdateThreadSleepTime);
            }
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
            var message = new OutgoingMessage
            {
                Broadcasts = EmsServerEndpoint.Flush(),
                PlayerUpdate = new PlayerUpdate(GameObject.Components)
            };
            Writer.Write(JsonConvert.SerializeObject(message));
        }
    }
}
