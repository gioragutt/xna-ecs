using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameManager GameManager { get; }

        private Transform entityTransform;

        public XnaTryGame()
        {
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GameManager = new GameManager();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var entity = GameManager.CreateGameObject();
            entity.Components.Add(new Sprite("Player/Down_002"));

            entity.Transform.Position = new Vector2(350, 0);
            entity.Transform.Scale = 0.2f;
            //entity.Transform.Rotation = MathHelper.ToRadians(60);

            entityTransform = entity.Transform;
            //entity.Components.Add(new RotateToMouse());
            entity.Components.Add(new Velocity(new Vector2(4)));

            var input = new KeyboardDirectionalInput();
            entity.Components.Add(input);

            var another = GameManager.CreateGameObject();
            another.Components.Add(new Sprite("Player/Down_003"));

            another.Transform.Position = new Vector2(700, 300);
            another.Transform.Scale = 0.2f;
            //another.Components.Add(new RotateToMouse());
            another.Components.Add(new Velocity(new Vector2(5)));
            another.Components.Add(new KeyboardDirectionalInput(new KeyboardSettings(Keys.A, Keys.D, Keys.W, Keys.S)));


            GameManager.CreateDebugPrint(input, Color.Green);
            GameManager.CreateDebugPrint(entity.Transform, Color.Red);
            GameManager.CreateDebugPrint(() => Mouse.GetState().ToString());

            GameManager.RegisterSystem(new MovementSystem());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameManager.RegisterDrawingSystem(new RendererSystem
            {
                Content = Content,
                SpriteBatch = spriteBatch
            });

            GameManager.RegisterDrawingSystem(new DebugPrintSystem
            {
                SpriteBatch = spriteBatch,
                Font = Content.Load<SpriteFont>("DefaultFont")
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
            if (keyboardState.KeysPressed(Keys.LeftShift, Keys.Q, Keys.W))
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
