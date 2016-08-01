using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace XnaTryLib
{
    public class BaseSprite : DrawableGameComponent
    {
        #region Properties

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public string SpriteAsset { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }

        #endregion

        public BaseSprite(Game game, string spriteAsset, Vector2 position)
            : this(game, null, position, null, spriteAsset, Constants.DEFAULT_SCALE, Constants.DEFAULT_ROTATION)
        {

        }

        public BaseSprite(Game game, Texture2D texture, Vector2 position, SpriteBatch spriteBatch, string spriteAsset, float scale, float rotation) 
            : base(game)
        {
            Texture = texture;
            Position = position;
            SpriteBatch = spriteBatch ?? Game.GetService<SpriteBatch>();
            SpriteAsset = spriteAsset;
            Scale = scale;
            Rotation = rotation;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            if (SpriteBatch == null)
                SpriteBatch = Game.GetService<SpriteBatch>();

            if (Texture == null && !string.IsNullOrEmpty(SpriteAsset))
            {
                Texture = Game.Content.Load<Texture2D>(SpriteAsset);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Debug.WriteLine("BaseSprite Update. gameTime=" + gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Debug.WriteLine("BaseSprite Draw. gameTime=" + gameTime);

            SpriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}
