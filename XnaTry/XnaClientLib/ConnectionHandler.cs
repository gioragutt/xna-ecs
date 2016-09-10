using ECS.BaseTypes;
using EMS;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
            if (IsDisposed)
                return;

            Connection?.Client.Disconnect(true);
            Connection?.Close();
            Reader.Dispose();
            Writer.Dispose();
            IsDisposed = true;
        }

        #region Network Components and Properties

        /// <summary>
        /// The EMS Server Endpoint to receive and broadcasts from and to the server
        /// </summary>
        private readonly EmsServerEndpoint emsServerEndpoint;

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
        /// Stores the time of the last update from the server
        /// </summary>
        public DateTime LastUpdateTime { get; private set; }

        /// <summary>
        /// Stores the time between the last two update
        /// </summary>
        public TimeSpan LastPing { get; private set; }

        /// <summary>
        /// Manages the MessageFraming protocol
        /// </summary>
        private PacketProtocol PacketProtocol { get; }

        private readonly TimeoutTimer timeoutCounter;

        public bool IsDisposed { get; private set; }

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
            IsDisposed = false;
            timeoutCounter = new TimeoutTimer(Constants.Time.MaxTimeout);
            HostName = hostName;
            Port = port;
            ClientGameManager = gameManager;
            GameObject = null;
            UpdateThread = new Thread(ConnectionHandler_InteractWithServer);
            emsServerEndpoint = new EmsServerEndpoint();
            PacketProtocol = new PacketProtocol(0)
            {
                MessageArrived = PacketProtocol_MessageRecievedCallback
            };
        }

        #endregion

        #region API

        public string GetPing()
        {
            return IsDisposed
                ? "Server disconnected"
                : string.Format("{0} ms", Math.Ceiling(LastPing.TotalMilliseconds));
        }

        public void ConnectAndInitializeLocalPlayer(string name, string team)
        {
            if (Connection != null && Connection.Connected)
                throw new InvalidOperationException("Attempt to connect when already connected");

            var hostname = string.IsNullOrEmpty(HostName) ? "localhost" : HostName;

            Connection = new TcpClient(hostname, Port)
            {
                ReceiveTimeout = (int)Constants.Time.MaxTimeout.TotalMilliseconds
            };

            Reader = new BinaryReader(Connection.GetStream());
            Writer = new BinaryWriter(Connection.GetStream());

            try
            {
                WriteLoginDataToServer(name, team);
                ReadLoginResponseFromServer();
            }
            catch (Exception)
            {
                Dispose();
                return;
            }

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
                try
                {
                    if (HelperMethods.Receive(Connection, Reader, PacketProtocol))
                        timeoutCounter.Reset();
                    else
                        timeoutCounter.Update(DateTime.Now - LastUpdateTime);
                }
                catch (Exception)
                {
                    Dispose();
                }

                Thread.Sleep(Constants.Time.UpdateThreadSleepTime);
            }
        }

        private void PacketProtocol_MessageRecievedCallback(byte[] data)
        {
            if (data.Length == 0)
                return;

            var stringData = Encoding.UTF8.GetString(data);
            ProcessServerUpdate(stringData);
            WritePlayerData();
        }

        private void ProcessServerUpdate(string message)
        {
            UpdatePing();

            var incomingUpdate = JsonConvert.DeserializeObject<ServerToClientUpdateMessage>(message);
            emsServerEndpoint.BroadcastIncomingEvents(incomingUpdate.Broadcasts);

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
                Broadcasts = emsServerEndpoint.Flush(),
                PlayerUpdate = new PlayerUpdate(GameObject.Components)
            };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var wrapperMessage = PacketProtocol.WrapMessage(messageBytes);
            Writer.Write(wrapperMessage);
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
