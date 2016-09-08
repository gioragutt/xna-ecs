using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using UtilsLib.Consts;
using UtilsLib.Utility;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    /// <summary>
    /// The base sprite component of entities in the game
    /// </summary>
    public class Sprite : GuiComponent
    {
        public override int DrawOrder()
        {
            return Constants.GUI.DrawOrder.Player;
        }

        /// <summary>
        /// Name of the asset you want to load
        /// </summary>
        /// <remarks>
        /// If the Texture property is not null, the asset specified will not be loaded
        /// </remarks>
        private string AssetName { get; }

        /// <summary>
        /// The texture to be Rendered
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The origin of rotation of the texture
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                if (Texture == null)
                    return Vector2.Zero;

                return new Vector2(Texture.Width / 2.0f, Texture.Height / 2.0f);
            }
        }

        /// <summary>
        /// Initializes a Sprite component with an asset name to load
        /// </summary>
        /// <param name="assetName">Name of the asset to load</param>
        /// <exception cref="System.ArgumentNullException">if assetName is empty or null</exception>
        public Sprite(string assetName)
        {
            Utils.AssertStringArgumentNotNull(assetName, "assetName");

            AssetName = assetName;
        }

        public override void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>(AssetName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var transform = Container.Get<Transform>();
            var spriteEffect = Container.Get<SpriteEffect>();

            if (Texture == null)
                return;

            ApplyEffectIfEnabled(spriteEffect);
            spriteBatch.Draw(
                texture: Texture,
                position: transform.Position,
                sourceRectangle: null, // draw whole texture; can be used for spritesheets
                color: Color.White,
                rotation: transform.Rotation,
                origin: Origin,
                scale: transform.Scale,
                effects: SpriteEffects.None,
                layerDepth: 0);
            DisableEffect(spriteEffect);
        }

        private static void DisableEffect(SpriteEffect spriteEffect)
        {
            if (IsEnabled(spriteEffect))
                spriteEffect.ResetPass();
        }

        private static void ApplyEffectIfEnabled(SpriteEffect spriteEffect)
        {
            if (IsEnabled(spriteEffect) && !string.IsNullOrEmpty(spriteEffect.AppliedPass))
                spriteEffect.Effect.CurrentTechnique.Passes[spriteEffect.AppliedPass].Apply();
        }
    }
}
