using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using UtilsLib;
using UtilsLib.Consts;
using UtilsLib.Exceptions.Server;
using UtilsLib.Utility;
using XnaClientLib;
using XnaClientLib.ECS;
using XnaClientLib.ECS.Compnents;
using XnaClientLib.ECS.Linkers;
using XnaClientLib.ECS.Systems;
using XnaCommonLib;
using XnaCommonLib.ECS;
using XnaCommonLib.ECS.Components;
using XnaCommonLib.ECS.Systems;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace XnaTry
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XnaTryGame : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        SpriteBatch spriteBatch;

        private readonly ClientGameManager clientGameManager;
        private readonly ResourcesManager resourceManager;
        private readonly ConnectionHandler connectionHandler;
        private readonly GraphicalUserInterface gui;

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        public TeamData goodTeam = new TeamData
        {
            Color = Color.Blue,
            Name = "Good",
            Frame = "Player/GUI/GreenTeam"
        };

        public TeamData badTeam = new TeamData
        {
            Color = Color.Red,
            Name = "Bad",
            Frame = "Player/GUI/RedTeam"
        };


        public Dictionary<string, TeamData> Teams { get; }

        public void InitializeGameSettings(int width, int height)
        {
            Graphics.PreferredBackBufferHeight = height;
            Graphics.PreferredBackBufferWidth = width;
            Graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        public XnaTryGame(IList<string> args)
        {
            Teams = new Dictionary<string, TeamData>
            {
                [goodTeam.Name] = goodTeam,
                [badTeam.Name] = badTeam
            };

            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            resourceManager = new ResourcesManager();
            clientGameManager = new ClientGameManager(resourceManager)
            {
                Teams = Teams
            };

            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = currentKeyboardState;

            var connectionArgs = new ConnectionArguments(args);
            connectionHandler = new ConnectionHandler(connectionArgs.Hostname, 27015, clientGameManager);
            ConnectToServer(connectionArgs.Name, connectionArgs.TeamName);

            gui = new GraphicalUserInterface(clientGameManager, "xna_try_map1.tmx", GetPing);
        }

        #region Ping

        private string GetPing()
        {
            return connectionHandler.IsDisposed
                ? "Server disconnected"
                : string.Format("{0} ms", Math.Ceiling(connectionHandler.LastPing.TotalMilliseconds));
        }

        #endregion

        #region Contants Configurations

        public readonly KeyboardLayoutOptions wasdKeys = new KeyboardLayoutOptions(Keys.A, Keys.D, Keys.W, Keys.S);
        public readonly KeyboardLayoutOptions arrowKeys = KeyboardDirectionalInput.DefaultLayoutOptions;
        public readonly KeyboardLayoutOptions numpadArrowKeys = new KeyboardLayoutOptions(Keys.NumPad4, Keys.NumPad6, Keys.NumPad8, Keys.NumPad2);

        #endregion

        GameObject CreatePlayer(DirectionalInput input, Vector2 initialPosition, TeamData team, string name = null, float health = 50)
        {
            // Create an entity
            var entity = clientGameManager.CreateGameObject();
            var components = entity.Components;

            // Now add input
            components.Add(new Velocity(new Vector2(2)));
            components.Add(input);

            // Show a character
            var sprite = resourceManager.Register(new Sprite("Player/Images/Down_001"));
            components.Add(sprite);

            // Change Transform
            entity.Transform.Scale = 0.4f;
            entity.Transform.Position = initialPosition;

            // Add Animation
            const long msPerFrame = 100;
            var stateAnimation = new StateAnimation<MovementDirection>(MovementDirection.Down,
                new Dictionary<MovementDirection, Animation>
                {
                    { MovementDirection.Down,  resourceManager.Register(new TextureCollectionAnimation(components, Utils.FormatRange("Player/Images/Down_{0:D3}", 1, 4), msPerFrame)) },
                    { MovementDirection.Up,  resourceManager.Register(new TextureCollectionAnimation(components, Utils.FormatRange("Player/Images/Up_{0:D3}", 1, 4), msPerFrame)) },
                    { MovementDirection.Left,  resourceManager.Register(new TextureCollectionAnimation(components, Utils.FormatRange("Player/Images/Left_{0:D3}", 1, 4), msPerFrame)) },
                    { MovementDirection.Right,  resourceManager.Register(new TextureCollectionAnimation(components, Utils.FormatRange("Player/Images/Right_{0:D3}", 1, 4), msPerFrame)) }
                });

            components.Add(stateAnimation);

            // Link Input to Animation
            components.Add(new MovementToAnimationLinker(entity.Components, stateAnimation));

            var attributes = new PlayerAttributes
            {
                MaxHealth = 100,
                Health = health,
                Name = name ?? entity.Entity.Id.ToString().Substring(0, 6),
                Team = team
            };

            components.Add(attributes);

            components.Add(resourceManager.Register(new PlayerStatusBar(components, Constants.Assets.PlayerHealthBar,
                Constants.Assets.PlayerNameFont)));

            return entity;
        }

        void CreateStupidAiPlayer()
        {
            var rnd = new Random();
            var isGoodTeam = Convert.ToBoolean(rnd.Next(0, 2));
            var team = isGoodTeam ? goodTeam : badTeam;

            var initialX = rnd.Next(100, 700);
            var initialY = rnd.Next(100, 400);
            var initialPosition = new Vector2(initialX, initialY);

            var health = (float)rnd.NextDouble() * 100;
            var aiPlayer = CreatePlayer(new FakeInput(), initialPosition, team, null, health);
            aiPlayer.Components.Get<PlayerAttributes>().Name = "[AI] " + aiPlayer.Components.Get<PlayerAttributes>().Name;
            aiPlayer.Destroy(5000);
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            InitializeGameSettings(1024, 768);

            gui.Initialize(Graphics.GraphicsDevice);
            clientGameManager.Camera.Bounds = gui.Map.Bounds;

            clientGameManager.RegisterDrawingSystem(new GuiComponentsSystem(spriteBatch, clientGameManager.Camera));
            clientGameManager.RegisterSystem(new LinkerSystem());
            clientGameManager.RegisterSystem(new LifespanSystem());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            var defaultFont = Content.Load<SpriteFont>("DefaultFont");

            resourceManager.SetContentManager(Content);
            resourceManager.LoadContent();
            clientGameManager.RegisterDrawingSystem(new DebugPrintSystem(spriteBatch, defaultFont));
            clientGameManager.RegisterDrawingSystem(new AnimationSystem());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (currentKeyboardState.KeysPressed(Keys.LeftControl, Keys.Q, Keys.W))
                Exit();

            if (currentKeyboardState.IsKeyDown(Keys.NumPad1) && !previousKeyboardState.IsKeyDown(Keys.NumPad1))
                connectionHandler.Broadcast(
                    MessageBuilder.Create(Constants.Messages.DamagePlayers)
                        .Add("damage", 25)
                        .Get());

            if (currentKeyboardState.IsKeyDown(Keys.NumPad2) && !previousKeyboardState.IsKeyDown(Keys.NumPad2))
                connectionHandler.Broadcast(
                    MessageBuilder.Create(Constants.Messages.DamagePlayers)
                        .Add(Constants.Fields.PlayerGuid, connectionHandler.GameObject.Entity.Id)
                        .Get());

            if (currentKeyboardState.IsKeyDown(Keys.P) && !previousKeyboardState.IsKeyDown(Keys.P))
                CreateStupidAiPlayer();

            resourceManager.LoadContent();
            clientGameManager.Update(gameTime, GraphicsDevice.Viewport);

            var title = string.Format("{2} | XnaTryGame | {0} | {1}",
                connectionHandler.GameObject.Components.Get<PlayerAttributes>().Name,
                connectionHandler.GameObject.Entity.Id, DateTime.Now);
            Window.Title = title;

            base.Update(gameTime);
        }

        private void ConnectToServer(string name, string team)
        {
            try
            {
                connectionHandler.ConnectAndInitializeLocalPlayer(name, team);
            }
            catch (Exception x)
            {
                if (ShowConnectionFailedError(x) == DialogResult.Yes)
                    throw new ConnectionEstablishmentErrorException("Failed to connect the server", x);
                Exit();
            }
        }

        private DialogResult ShowConnectionFailedError(Exception x)
        {
            return MessageBox.Show(
                string.Format("Failed to connect the server {0}:{1}{2}{3}{2}Throw exception for debug?", connectionHandler.HostName, connectionHandler.Port, Environment.NewLine, x), 
                "Connection failed",
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button2);
        }

        /// <summary>
        /// Close TCP Connection when game ends
        /// </summary>
        protected override void EndRun()
        {
            connectionHandler.Dispose();
            base.EndRun();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var background = new Color(96, 88, 88);
            GraphicsDevice.Clear(background);
            clientGameManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
