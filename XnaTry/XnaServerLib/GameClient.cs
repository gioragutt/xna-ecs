using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ECS.Interfaces;
using EMS;
using Microsoft.Xna.Framework;
using XnaCommonLib;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaServerLib.ECS;
using Constants = XnaCommonLib.Constants;

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
            Util.AssertArgumentNotNull(connection, "connection");
            Util.AssertArgumentNotNull(gameObject, "gameObject");

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
                    ReadPlayerData();
                    WriteAllPlayerData();
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

                Thread.Sleep(Constants.UpdateThreadSleepTime);
            }
            
            Console.WriteLine("Player {0} disconnected", GameObject.Components.Get<PlayerAttributes>().Name);

            Broadcast(
                MessageBuilder.Create(EventMessageNames.ClientDisconnected)
                    .Add(Constants.MessageFields.GuidField, GameObject.Entity.Id)
                    .Get());

            GameManager.DisposeOfClient(this);
            GameObject = null;
        }

        private void ReadPlayerData()
        {
            EmsServerEndpoint.BroadcastIncomingEvents(Reader);
            var components = GameObject.Components;
            components.Get<DirectionalInput>().Read(Reader);
        }

        private void WriteAllPlayerData()
        {
            EmsServerEndpoint.Flush(Writer);
            var amountOfPlayers = GameManager.EntitiesCount;
            Writer.Write(amountOfPlayers);

            var allEntities = GameManager.EntityPool.AllThat(c => c.Has<Transform>() && c.Has<PlayerAttributes>() && c.Has<DirectionalInput>());

            foreach (var entity in allEntities)
            {
                WriteEntity(entity.Parent, entity);
            }
        }

        private void WriteEntity(IEntity entity, IComponentContainer entityComponents)
        {
            Util.WriterGuid(Writer, entity.Id);
            entityComponents.Get<Transform>().Write(Writer);
            entityComponents.Get<PlayerAttributes>().Write(Writer);
            entityComponents.Get<DirectionalInput>().Write(Writer);
            entityComponents.Get<Velocity>().Write(Writer);
        }

        private void SendClientLoginResponse()
        {
            WriteEntity(GameObject.Entity, GameObject.Components);
        }

        private void ReadClientLoginDataAndInitializePlayer()
        {
            var name = Reader.ReadString();
            var team = Reader.ReadString();

            GameObject.Transform.Scale = 0.4f;
            GameObject.Transform.Position = new Vector2(50, 300);

            var teamName = GameManager.EntitiesCount % 2 == 0 ? "Bad Team" : "Good Team";

            GameObject.Components.Add(new PlayerAttributes
            {
                Name = GameManager.GetAvailablePlayerName(name),
                Team = new TeamData
                {
                    Name = teamName,
                },
                MaxHealth = 100,
                Health = 50
            });

            GameObject.Components.Add(new InputData());
            GameObject.Components.Add(new Velocity(new Vector2(500)));
        }
    }
}