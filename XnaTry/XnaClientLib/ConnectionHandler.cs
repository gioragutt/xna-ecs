using System;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ECS.BaseTypes;
using XnaClientLib.ECS;
using XnaClientLib.ECS.Compnents;
using XnaCommonLib;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib
{
    public class ConnectionHandler : Component, IDisposable
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

        public ConnectionHandler(string hostName, int port, ClientGameManager gameManager)
        {
            HostName = hostName;
            Port = port;
            ClientGameManager = gameManager;
            GameObject = null;
            UpdateThread = new Thread(ConnectionHandler_InteractWithServer);
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
            var guidLength = Reader.ReadInt32();
            var guid = Encoding.ASCII.GetString(Reader.ReadBytes(guidLength));
            GameObject = ClientGameManager.CreateGameObject(Guid.Parse(guid));
            var attributes = new PlayerAttributes();
            attributes.Read(Reader);
            GameObject.Components.Add(attributes);
            GameObject.Transform.Read(Reader);
            var velocty = new Velocity(Vector2.Zero);
            velocty.Read(Reader);
            GameObject.Components.Add(velocty);
            ClientGameManager.InitializeLocalClient(GameObject);
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
                var playersUpdate = Reader.ReadInt32();
                Debug.WriteLine("Reading {0} Players", playersUpdate);

                for (var i = 0; i < playersUpdate; i++)
                {
                    var guid = Util.ReadString(Reader);
                    var entity = new Entity(Guid.Parse(guid));
                    if (!ClientGameManager.EntityPool.Exists(entity))
                    {
                        var newGo = ClientGameManager.AllocateGameObjectForRemote(entity.Id);
                        newGo.Components.Get<NetworkPlayer>().Update(Reader);
                        ClientGameManager.InitializeRemoteClient(newGo);
                    }
                    else
                    {
                        var remoteComponents = ClientGameManager.EntityPool.GetComponents(entity);
                        remoteComponents.Get<NetworkPlayer>().Update(Reader);
                    }
                }

                WritePlayerData();

                Thread.Sleep(Constants.UpdateThreadSleepTime);
            }
        }

        private void WritePlayerData()
        {
            var components = GameObject.Components;
            components.Get<DirectionalInput>().Write(Writer);
        }
    }
}
