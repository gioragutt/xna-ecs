using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaClientLib.ECS.Compnents;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Systems
{
    public class RendererSystem : XnaCommonLib.ECS.Systems.System
    {
        public SpriteBatch SpriteBatch { get; }

        public RendererSystem(SpriteBatch spriteBatch, bool enabled = true) 
            : base(enabled)
        {
            SpriteBatch = spriteBatch;
        }

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

            SpriteBatch.Draw(
                texture: sprite.Texture, 
                position: transform.Position, 
                sourceRectangle: null, // draw whole texture; can be used for spritesheets
                color: Color.White,
                rotation: transform.Rotation, 
                origin: sprite.Origin, 
                scale: transform.Scale, 
                effects: SpriteEffects.None, 
                layerDepth: 0);
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<Sprite>() && c.Has<Transform>()).ToList();
        }
    }
}
