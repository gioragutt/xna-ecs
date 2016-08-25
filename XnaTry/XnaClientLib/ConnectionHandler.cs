using ECS.BaseTypes;
using EMS;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using XnaClientLib.ECS;
using XnaClientLib.ECS.Compnents;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using UtilsLib;
using UtilsLib.Consts;

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
            UpdateThread.Start();
            WritePlayerData();
        }

        private void ReadLoginResponseFromServer()
        {
            GameObject = ClientGameManager.BeginAllocateLocal(Reader.ReadGuid());
            GameObject.Components.Get<NetworkPlayer>().Update(Reader);
            ClientGameManager.EndAllocate(GameObject);
        }

        private void WriteLoginDataToServer(string name, string team)
        {
            Writer.Write(name);
            Writer.Write(team);
        }

        private void ConnectionHandler_InteractWithServer()
        {
            while (Connection.Connected)
            {
                EmsServerEndpoint.BroadcastIncomingEvents(Reader);
                var playersUpdate = Reader.ReadInt32();
                Debug.WriteLine("Reading {0} Players", playersUpdate);

                for (var i = 0; i < playersUpdate; i++)
                {
                    var guid = Reader.ReadGuid();
                    var entity = new Entity(guid);
                    if (!ClientGameManager.EntityPool.Exists(entity))
                    {
                        var newGo = ClientGameManager.BeginAllocateRemote(entity.Id);
                        newGo.Components.Get<NetworkPlayer>().Update(Reader);
                        ClientGameManager.EndAllocate(newGo);
                    }
                    else
                    {
                        var remoteComponents = ClientGameManager.EntityPool.GetComponents(entity);
                        remoteComponents.Get<NetworkPlayer>().Update(Reader);
                    }
                }

                WritePlayerData();

                Thread.Sleep(Constants.Time.UpdateThreadSleepTime);
            }
        }

        private void WritePlayerData()
        {
            EmsServerEndpoint.Flush(Writer);
            var components = GameObject.Components;
            components.Get<DirectionalInput>().Write(Writer);
        }
    }
}
