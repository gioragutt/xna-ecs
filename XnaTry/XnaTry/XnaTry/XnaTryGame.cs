using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaTryLib;
using XnaTryLib.ECS;
using XnaTryLib.ECS.Components;
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

        public XnaTryGame()
        {
            IsMouseVisible = true;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GameManager = new GameManager();
        }

        public readonly KeyboardLayoutOptions wasdKeys = new KeyboardLayoutOptions(Keys.A, Keys.D, Keys.W, Keys.S);
        public readonly KeyboardLayoutOptions arrowKeys = KeyboardDirectionalInput.DefaultLayoutOptions;
        public readonly KeyboardLayoutOptions numpadArrowKeys = new KeyboardLayoutOptions(Keys.NumPad4, Keys.NumPad6, Keys.NumPad8, Keys.NumPad2);

        void Initialize_ECS_Example(KeyboardLayoutOptions keys, Color debugColor, Vector2 initialPosition)
        {
            bool CreateAnnoyingComponents = false;

            // Create an entity
            var entity = GameManager.CreateGameObject();

            // Now add input
            entity.Components.Add(new Velocity(new Vector2(5)));
            entity.Components.Add(new KeyboardDirectionalInput(keys));

            // Now show that there's an entity
            GameManager.CreateDebugPrint(entity.Transform, debugColor);

            // Show a character
            var sprite = new Sprite("Player/Down_001");
            entity.Components.Add(sprite);

            // Change Transform
            entity.Transform.Scale = 0.3f;
            entity.Transform.Position = initialPosition;

            if (CreateAnnoyingComponents)
                entity.Transform.Rotation = MathHelper.ToRadians(30);

            // Add Animation
            const long msPerFrame = 100; 
            var stateAnimation = new StateAnimation<MovementDirection>(sprite, 0, MovementDirection.Down,
                new Dictionary<MovementDirection, Animation>
                {
                    { MovementDirection.Down, new TextureCollectionAnimation(sprite, Util.FormatRange("Player/Down_{0:D3}", 1, 4), msPerFrame) },
                    { MovementDirection.Up, new TextureCollectionAnimation(sprite, Util.FormatRange("Player/Up_{0:D3}", 1, 4), msPerFrame) },
                    { MovementDirection.Left, new TextureCollectionAnimation(sprite, Util.FormatRange("Player/Left_{0:D3}", 1, 4), msPerFrame) },
                    { MovementDirection.Right, new TextureCollectionAnimation(sprite, Util.FormatRange("Player/Right_{0:D3}", 1, 4), msPerFrame) }
                });

            entity.Components.Add(stateAnimation);

            // Link Input to Animation
            entity.Components.Add(new MovementToAnimationLinker(entity.Components.Get<DirectionalInput>(), stateAnimation));
            entity.Components.Add(new Label(debugColor.ToString(), LabelPlacement.TopCenter, debugColor));

            if (CreateAnnoyingComponents)
            {
                // If you really like it, you can have some fun rotating your character towards the mouse
                entity.Components.Add(new RotateToMouse());
            }
        }

        protected override void Initialize()
        {
            base.Initialize();

            Initialize_ECS_Example(wasdKeys, Color.Red, new Vector2(100, 400));
            Initialize_ECS_Example(arrowKeys, Color.Blue, new Vector2(700, 400));

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

            var defaultFont = Content.Load<SpriteFont>("DefaultFont");

            GameManager.RegisterDrawingSystem(new AnimationSystem
            {
                Content = Content
            });

            GameManager.RegisterDrawingSystem(new RendererSystem
            {
                Content = Content,
                SpriteBatch = spriteBatch
            });

            GameManager.RegisterDrawingSystem(new LabelSystem
            {
                DefaultFont = defaultFont,
                SpriteBatch = spriteBatch
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
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            // Allows the game to exit
            if (keyboardState.KeysPressed(Keys.LeftControl, Keys.Q, Keys.W))
                Exit();

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
