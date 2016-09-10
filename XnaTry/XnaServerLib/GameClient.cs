using EMS;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SharedGameData;
using UtilsLib;
using UtilsLib.Consts;
using UtilsLib.Utility;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaCommonLib.Network;
using XnaServerLib.ECS;

namespace XnaServerLib
{
    public class GameClient : EmsClient, IDisposable
    {
        public void Dispose()
        {
            if (Connection.Connected)
            {
                Connection?.Client.Disconnect(true);
                Connection?.Close();
            }
            Reader.Dispose();
            Writer.Dispose();
            GameObject.Entity.Dispose();
            GameObject = null;
        }

        #region Properties

        /// <summary>
        /// The TcpClient object with which connection is established
        /// </summary>
        private TcpClient Connection { get; }

        /// <summary>
        /// Used for reading data sent from the client
        /// </summary>
        public BinaryReader Reader { get; }

        /// <summary>
        /// Used to send data to the client
        /// </summary>
        public BinaryWriter Writer { get; }

        /// <summary>
        /// The GameObject allocated for the client
        /// </summary>
        public GameObject GameObject { get; private set; }

        /// <summary>
        /// The servers game manager
        /// </summary>
        public ServerGameManager GameManager { get; set; }

        /// <summary>
        /// The thread on which the the GameClient interacts with the connected client
        /// </summary>
        private Thread UpdateThread { get; }

        /// <summary>
        /// The EmsServer Endpoint
        /// </summary>
        private EmsServerEndpoint EmsServerEndpoint { get; }

        private PacketProtocol PacketProtocol { get; }

        /// <summary>
        /// Stores the time of the last update from the client
        /// </summary>
        public DateTime LastUpdateTime { get; private set; }

        private TimeoutTimer TimeoutTimer { get; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new GameClient
        /// </summary>
        /// <param name="connection">The TcpClient with which connection was established</param>
        /// <param name="gameObject">The GameObject allocated for the client</param>
        /// <param name="gameManager">The server's game manager</param>
        public GameClient(TcpClient connection, GameObject gameObject, ServerGameManager gameManager)
        {
            Utils.AssertArgumentNotNull(connection, "connection");
            Utils.AssertArgumentNotNull(gameObject, "gameObject");

            EmsServerEndpoint = new EmsServerEndpoint();
            TimeoutTimer = new TimeoutTimer(Constants.Time.MaxTimeout);
            GameObject = gameObject;
            GameManager = gameManager;
            Connection = connection;

            var connectionStream = Connection.GetStream();
            Reader = new BinaryReader(connectionStream);
            Writer = new BinaryWriter(connectionStream);

            try
            {
                ReadClientLoginDataAndInitializePlayer();
                SendClientLoginResponse();
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }

            PacketProtocol = new PacketProtocol(0)
            {
                MessageArrived = PacketProtocol_MessageArrivedCallback
            };

            UpdateThread = new Thread(GameClient_InteractWithClient);
            UpdateThread.Start();
        }

        #endregion Constructor

        private void GameClient_InteractWithClient()
        {
            while (Connection.Connected)
            {
                try
                {
                    if (HelperMethods.Receive(Connection, Reader, PacketProtocol))
                        TimeoutTimer.Reset();
                    else
                        TimeoutTimer.Update(DateTime.Now - LastUpdateTime);
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} caught for {1}\n{2}", e.GetType().FullName, GameObject.Components.Get<PlayerAttributes>().Name, e);
                    break;
                }

                Thread.Sleep(Constants.Time.UpdateThreadSleepTime);
            }

            var playerName = GameObject.Components.Get<PlayerAttributes>().Name;
            Console.WriteLine("{1}\nPlayer <<< {0} >>> disconnected\n{1}", playerName, "============================================");

            Broadcast(
                MessageBuilder.Create(Constants.Messages.ClientDisconnected)
                    .Add(Constants.Fields.PlayerGuid, GameObject.Entity.Id)
                    .Add(Constants.Fields.PlayerName, playerName)
                    .Get());

            GameManager.DisposeOfClient(this);
        }

        private void PacketProtocol_MessageArrivedCallback(byte[] bytes)
        {
            LastUpdateTime = DateTime.Now;
            if (bytes.Length == 0)
                return;

            var stringData = Encoding.UTF8.GetString(bytes);
            ProcessClientUpdate(stringData);
            SendServerUpdate();
        }

        private void SendServerUpdate()
        {
            var message = JsonConvert.SerializeObject(new ServerToClientUpdateMessage
            {
                Broadcasts = EmsServerEndpoint.Flush(),
                PlayerUpdates = PlayerUpdates()
            });

            var messageBytes = Encoding.UTF8.GetBytes(message);
            Writer.Write(PacketProtocol.WrapMessage(messageBytes));
        }

        private void ProcessClientUpdate(string clientMessageString)
        {
            var clientMessage = JsonConvert.DeserializeObject<ClientToServerUpdateMessage>(clientMessageString);
            EmsServerEndpoint.BroadcastIncomingEvents(clientMessage.Broadcasts);
            UpdateClient(clientMessage.PlayerUpdate);
        }

        private IList<PlayerUpdate> PlayerUpdates()
        {
            return GameManager.EntityPool.AllThat(PlayerUpdate.IsPlayer).Select(c => new PlayerUpdate(c)).ToList();
        }

        private void UpdateClient(PlayerUpdate playerUpdate)
        {
            var components = GameObject.Components;
            components.Get<DirectionalInput>().Update(playerUpdate.Input);
        }

        private void SendClientLoginResponse()
        {
            var responseMessage = new PlayerUpdate(GameObject.Components);
            Writer.Write(JsonConvert.SerializeObject(responseMessage));
        }

        private static string RandomTeam()
        {
            var rnd = new Random();
            var values = TeamsData.Teams.Keys;
            return values.ToList()[rnd.Next(0, values.Count)];
        }

        private void ReadClientLoginDataAndInitializePlayer()
        {
            LastUpdateTime = DateTime.Now;
            var serializedMessage = Reader.ReadString();
            var loginMessage = JsonConvert.DeserializeObject<ClientLoginMessage>(serializedMessage);
            var teamName = TeamsData.Teams.ContainsKey(loginMessage.PlayerTeam) ? loginMessage.PlayerTeam : RandomTeam();

            GameObject.Transform.Scale = 0.4f;
            GameObject.Transform.Position = GameManager.Server.MapManager.GetRandomSpawnPosition(teamName);

            GameObject.Components.Add(new PlayerAttributes
            {
                Name = GameManager.GetAvailablePlayerName(loginMessage.PlayerName),
                Team = TeamsData.Teams[teamName],
                MaxHealth = 100,
                Health = 50
            });

            GameObject.Components.Add(new InputData());
            GameObject.Components.Add(new Velocity(new Vector2(500)));
        }
    }
}