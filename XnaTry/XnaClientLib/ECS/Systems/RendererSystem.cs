using System;
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
        public Camera Camera { get; }

        public RendererSystem(SpriteBatch spriteBatch, Camera camera, bool enabled = true) 
            : base(enabled)
        {
            SpriteBatch = spriteBatch;
            Camera = camera;
        }

        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            for (var index = 0; index < entities.Count; index++)
                DrawEntity(entities[index]);
        }

        private void DrawEntity(IComponentContainer entity)
        {
            var sprite = entity.Get<Sprite>();
            var transform = entity.Get<Transform>();
            var spriteEffect = entity.Get<SpriteEffect>();

            if (sprite.Texture == null)
                return;

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.CameraMatrix);
            ApplyEffectIfEnabled(spriteEffect);
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
            DisableEffect(spriteEffect);
            SpriteBatch.End();
        }

        private static void DisableEffect(SpriteEffect spriteEffect)
        {
            if (Component.IsEnabled(spriteEffect))
                spriteEffect.ResetPass();
        }

        private static void ApplyEffectIfEnabled(SpriteEffect spriteEffect)
        {
            if (Component.IsEnabled(spriteEffect) && !string.IsNullOrEmpty(spriteEffect.AppliedPass))
                spriteEffect.Effect.CurrentTechnique.Passes[spriteEffect.AppliedPass].Apply();
        }

        public override Predicate<IComponentContainer> RelevantEntities()
        {
            return c => c.Has<Sprite>() && c.Has<Transform>();
        }
    }
}
