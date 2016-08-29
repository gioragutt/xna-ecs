using EMS;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json.Linq;
using UtilsLib;
using UtilsLib.Consts;
using UtilsLib.Utility;
using XnaCommonLib;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaCommonLib.Network;
using XnaServerLib.ECS;
using XnaServerLib.ECS.Components;

namespace XnaServerLib
{
    public class GameClient : EmsClient, IDisposable
    {
        public void Dispose()
        {
            if (Connection != null && Connection.Connected)
                Connection.Close();
            Reader.Dispose();
            Writer.Dispose();
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

            GameObject = gameObject;
            GameManager = gameManager;
            Connection = connection;

            var connectionStream = Connection.GetStream();
            Reader = new BinaryReader(connectionStream);
            Writer = new BinaryWriter(connectionStream);

            ReadClientLoginDataAndInitializePlayer();
            SendClientLoginResponse();

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
                    var clientMessageString = Reader.ReadString();
                    var clientMessage = JsonConvert.DeserializeObject<OutgoingMessage>(clientMessageString);
                    EmsServerEndpoint.BroadcastIncomingEvents(clientMessage.Broadcasts);
                    UpdateClient(clientMessage.PlayerUpdate);
                    Writer.Write(JsonConvert.SerializeObject(new IncomingUpdate
                    {
                        Broadcasts = EmsServerEndpoint.Flush(),
                        PlayerUpdates = PlayerUpdates()
                    }));
                }
                catch (IOException)
                {
                    Console.WriteLine("IOException caught for {0}", GameObject.Components.Get<PlayerAttributes>().Name);
                    Connection.Close();
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Exception caught for {0}", GameObject.Components.Get<PlayerAttributes>().Name);
                    Connection.Close();
                    break;
                }

                Thread.Sleep(Constants.Time.UpdateThreadSleepTime);
            }
            
            Console.WriteLine("Player {0} disconnected", GameObject.Components.Get<PlayerAttributes>().Name);

            Broadcast(
                MessageBuilder.Create(Constants.Messages.ClientDisconnected)
                    .Add(Constants.Fields.PlayerGuid, GameObject.Entity.Id)
                    .Get());

            GameManager.DisposeOfClient(this);
            GameObject = null;
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

        private void ReadClientLoginDataAndInitializePlayer()
        {
            var serializedMessage = Reader.ReadString();
            var loginMessage = JsonConvert.DeserializeObject<JObject>(serializedMessage);

            if (loginMessage.GetMessageName() != Constants.Messages.PlayerLogin)
                Dispose();

            GameObject.Transform.Scale = 0.4f;
            GameObject.Transform.Position = new Vector2(50, 300);

            GameObject.Components.Add(new PlayerAttributes
            {
                Name = GameManager.GetAvailablePlayerName(loginMessage.GetProp<string>(Constants.Fields.PlayerName)),
                Team = new TeamData
                {
                    Name = loginMessage.GetProp<string>(Constants.Fields.TeamName)
                },
                MaxHealth = 100,
                Health = 50
            });

            GameObject.Components.Add(new InputData());
            GameObject.Components.Add(new Velocity(new Vector2(5)));
            GameObject.Components.Add(new BoxCollision(GameManager.World,
                ConvertUnits.ToSimUnits(new Vector2(155, 365) * GameObject.Transform.Scale),
                GameObject.Transform.Position));
        }
    }
}