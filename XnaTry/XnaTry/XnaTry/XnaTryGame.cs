using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaTry.ECS.Components;
using XnaTryLib;
using XnaTryLib.ECS;
using XnaTryLib.ECS.Components;
using XnaTryLib.ECS.Linkers;
using XnaTryLib.ECS.Systems;

namespace XnaTry
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XnaTryGame : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        SpriteBatch spriteBatch;
        private GameManager GameManager { get; }
        private ResourcesManager ResourceManager { get; }
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        public XnaTryGame()
        {
            IsMouseVisible = true;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GameManager = new GameManager();
            ResourceManager = new ResourcesManager();
            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = currentKeyboardState;
        }

        #region Contants Configurations

        public readonly KeyboardLayoutOptions wasdKeys = new KeyboardLayoutOptions(Keys.A, Keys.D, Keys.W, Keys.S);
        public readonly KeyboardLayoutOptions arrowKeys = KeyboardDirectionalInput.DefaultLayoutOptions;
        public readonly KeyboardLayoutOptions numpadArrowKeys = new KeyboardLayoutOptions(Keys.NumPad4, Keys.NumPad6, Keys.NumPad8, Keys.NumPad2);

        public readonly TeamData goodTeam = new TeamData
        {
            Color = Color.Blue,
            Name = "Good Team",
            TeamFrameTextureAsset = "Player/GUI/GreenTeam"
        };

        public readonly TeamData badTeam = new TeamData
        {
            Color = Color.Red,
            Name = "Bad Team",
            TeamFrameTextureAsset = "Player/GUI/RedTeam"
        };

        #endregion

        GameObject CreatePlayer(DirectionalInput input, Vector2 initialPosition, TeamData team, string name = null, float health = 50)
        {
            // Create an entity
            var entity = GameManager.CreateGameObject();
            var components = entity.Components;

            // Now add input
            components.Add(new Velocity(new Vector2(2)));
            components.Add(input);

            // Show a character
            var sprite = ResourceManager.Register(new Sprite("Player/Images/Down_001"));
            components.Add(sprite);

            // Change Transform
            entity.Transform.Scale = 0.4f;
            entity.Transform.Position = initialPosition;

            // Add Animation
            const long msPerFrame = 100;
            var stateAnimation = new StateAnimation<MovementDirection>(sprite, 0, MovementDirection.Down,
                new Dictionary<MovementDirection, Animation>
                {
                    { MovementDirection.Down,  ResourceManager.Register(new TextureCollectionAnimation(sprite, Util.FormatRange("Player/Images/Down_{0:D3}", 1, 4), msPerFrame)) },
                    { MovementDirection.Up,  ResourceManager.Register(new TextureCollectionAnimation(sprite, Util.FormatRange("Player/Images/Up_{0:D3}", 1, 4), msPerFrame)) },
                    { MovementDirection.Left,  ResourceManager.Register(new TextureCollectionAnimation(sprite, Util.FormatRange("Player/Images/Left_{0:D3}", 1, 4), msPerFrame)) },
                    { MovementDirection.Right,  ResourceManager.Register(new TextureCollectionAnimation(sprite, Util.FormatRange("Player/Images/Right_{0:D3}", 1, 4), msPerFrame)) }
                });

            components.Add(stateAnimation);

            // Link Input to Animation
            components.Add(new MovementToAnimationLinker(entity.Components.Get<DirectionalInput>(), stateAnimation));

            var attributes = new PlayerAttributes
            {
                MaxHealth = 100,
                Health = health,
                Name = name ?? entity.Entity.Id.ToString().Substring(0, 6),
                Team = team
            };

            components.Add(attributes);

            components.Add(ResourceManager.Register(new PlayerStatusBar(attributes, sprite, entity.Transform, "Player/GUI/HealthBar",
                "Player/Fonts/NameFont")));

            return entity;
        }

        void CreatePlayerControllerPlayer(KeyboardLayoutOptions keys, Vector2 initialPosition, string name, TeamData team)
        {
            var playerControllerPlayer = CreatePlayer(new KeyboardDirectionalInput(keys), initialPosition, team, name);
            playerControllerPlayer.Components.Get<PlayerAttributes>().Name = "[PC] " + playerControllerPlayer.Components.Get<PlayerAttributes>().Name;
        }

        void CreateStupidAIPlayer()
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
        }

        protected override void Initialize()
        {
            base.Initialize();

            GameManager.CreateDebugPrint(() => GameManager.ToString());

            CreatePlayerControllerPlayer(arrowKeys, new Vector2(400, 400), "world", goodTeam);
            CreatePlayerControllerPlayer(wasdKeys, new Vector2(100, 400), "hello", badTeam);

            GameManager.RegisterSystem(new MovementSystem());
            GameManager.RegisterSystem(new LinkerSystem());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceManager.SetContentManager(Content);

            var defaultFont = Content.Load<SpriteFont>("DefaultFont");

            GameManager.RegisterDrawingSystem(new AnimationSystem());

            GameManager.RegisterDrawingSystem(new RendererSystem
            {
                SpriteBatch = spriteBatch
            });

            GameManager.RegisterDrawingSystem(new GuiComponentsSystem
            {
                SpriteBatch = spriteBatch,
            });

            GameManager.RegisterDrawingSystem(new DebugPrintSystem
            {
                SpriteBatch = spriteBatch,
                Font = defaultFont
            });

            base.LoadContent();
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

            if (currentKeyboardState.IsKeyDown(Keys.P) && !previousKeyboardState.IsKeyDown(Keys.P))
                CreateStupidAIPlayer();

            ResourceManager.LoadContent();
            GameManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GameManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
