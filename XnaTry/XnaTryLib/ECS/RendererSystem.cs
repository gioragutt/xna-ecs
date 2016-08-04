using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTryLib.ECS
{
    public class RendererSystem : BaseSystem<Sprite>
    {
        public SpriteBatch SpriteBatch { get; set; }
        public ContentManager Content { get; set; }

        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            SpriteBatch.Begin();
            foreach (var entity in entities)
            {
                UpdateEntity(entity);
            }
            SpriteBatch.End();
        }

        private void UpdateEntity(IComponentContainer entity)
        {
            var sprite = entity.Get<Sprite>();
            var transform = entity.Get<Transform>();
            LoadSpriteIfNeeded(sprite);

            SpriteBatch.Draw(
                texture: sprite.Texture, 
                position: transform.Position, 
                sourceRectangle: null, // draw whole texture; can be used for spritesheets
                color: Color.White,
                rotation: transform.Rotation, 
                origin: Vector2.Zero, 
                scale: transform.Scale, 
                effects: SpriteEffects.None, 
                layerDepth: 0);
        }

        /// <summary>
        /// A sprite that fall under the GetRelevant predicate might be new and don't have
        /// A texture loaded. If the texture is not loaded, the sprite must have a valid AssetName property
        /// </summary>
        /// <param name="sprite">The sprite updated</param>
        private void LoadSpriteIfNeeded(Sprite sprite)
        {
            Util.AssertArgumentNotNull(sprite, "sprite");
            if (sprite.Texture != null)
                return;
            Util.AssertStringArgumentNotNull(sprite.AssetName, "sprite.AssetName");
            sprite.Texture = Content.Load<Texture2D>(sprite.AssetName);
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<Sprite>() && c.Has<Transform>()).ToList();
        }
    }
}
