using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using XnaCommonLib;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaServerLib.ECS;

namespace XnaServerLib
{
    public class GameClient : IDisposable
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

            GameObject = gameObject;
            GameManager = gameManager;
            Connection = connection;

            var connectionStream = Connection.GetStream();
            Reader = new BinaryReader(connectionStream);
            Writer = new BinaryWriter(connectionStream);

            ReadClientLoginData();
            SendClientLoginResponse();
            UpdateThread = new Thread(GameClient_InteractWithClient);
            UpdateThread.Start();
        }

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

            GameObject.Entity.Dispose();
            GameObject = null;
        }

        private void ReadPlayerData()
        {
            Console.WriteLine("Writing to client");
            var components = GameObject.Components;
            components.Get<Transform>().Read(Reader);
            components.Get<DirectionalInput>().Read(Reader);
        }

        private void WriteAllPlayerData()
        {
            var amountOfPlayers = GameManager.EntitiesCount;
            Writer.Write(amountOfPlayers);
            Console.WriteLine("Writing {0} Players", amountOfPlayers);

            var allEntities = GameManager.EntityPool.AllEntities;

            foreach (var entity in allEntities)
            {
                var entityComponents = GameManager.EntityPool.GetComponents(entity);
                Util.WriteString(Writer, entity.Id.ToString());
                entityComponents.Get<Transform>().Write(Writer);
                entityComponents.Get<PlayerAttributes>().Write(Writer);
                entityComponents.Get<DirectionalInput>().Write(Writer);
            }
        }

        private void SendClientLoginResponse()
        {
            var guid = Encoding.ASCII.GetBytes(GameObject.Entity.Id.ToString());
            Writer.Write(guid.Length);
            Writer.Write(guid);
            GameObject.Components.Get<PlayerAttributes>().Write(Writer);
            GameObject.Transform.Position = new Vector2(50, 300);
            GameObject.Transform.Write(Writer);
        }

        private void ReadClientLoginData()
        {
            var name = Reader.ReadString();
            var team = Reader.ReadString();

            GameObject.Components.Add(new PlayerAttributes
            {
                Name = name,
                Team = new TeamData
                {
                    Name = team,
                },
                MaxHealth = 100,
                Health = 50
            });

            GameObject.Components.Add(new InputData());
        }

        #endregion Constructor
    }
}