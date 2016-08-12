using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaTryLib.ECS.Components;

namespace XnaTryLib.ECS.Systems
{
    public class RendererSystem : System
    {
        public SpriteBatch SpriteBatch { get; set; }

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
