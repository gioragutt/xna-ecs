using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaTryLib;
using XnaTryLib.ECS;

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

        public XnaTryGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            GameManager = new GameManager();
            var entity = GameManager.CreateGameObject();
            entity.Components.Add(new Sprite("Front_2"));
            entity.Transform.Position = new Vector2(350, 0);
            entity.Transform.Scale = 0.5f;
            entity.Transform.Rotation = MathHelper.ToRadians(90);
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
            if (keyboardState.IsKeyDown(Keys.LeftAlt) && keyboardState.IsKeyDown(Keys.F4))
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
