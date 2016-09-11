using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using XnaClientLib.ECS.Compnents;
using XnaClientLib.ECS.Compnents.GUI;

namespace XnaClientLib.ECS.Systems
{
    public class GuiComponentsSystem : XnaCommonLib.ECS.Systems.System
    {
        public SpriteBatch SpriteBatch { get; }
        public Camera Camera { get; }
        public Viewport DefaultViewport { get; }

        public GuiComponentsSystem(SpriteBatch spriteBatch, Camera camera, bool enabled = true) 
            : base(enabled)
        {
            SpriteBatch = spriteBatch;
            Camera = camera;
            DefaultViewport = SpriteBatch.GraphicsDevice.Viewport;
        }

        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            var allGuiComponents = entities.SelectMany(c => c.GetAllOf<GuiComponent>()).ToList();
            allGuiComponents.Sort((first, second) => first.DrawOrder.CompareTo(second.DrawOrder));
            foreach (var guiComponent in allGuiComponents)
            {
                SetViewport(guiComponent);
                SpriteBatchBegin(guiComponent);
                guiComponent.Update(TimeSpan.FromMilliseconds(delta));
                guiComponent.Draw(SpriteBatch);
                SpriteBatch.End();
            }
            ResetViewport();
        }

        private void ResetViewport()
        {
            SpriteBatch.GraphicsDevice.Viewport = DefaultViewport;
        }

        private void SetViewport(GuiComponent guiComponent)
        {
            SpriteBatch.GraphicsDevice.Viewport = guiComponent.Viewport ?? DefaultViewport;
        }

        private void SpriteBatchBegin(GuiComponent guiComponent)
        {
            if (guiComponent.IsHud)
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            else
                SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.CameraMatrix);
        }

        public override Predicate<IComponentContainer> RelevantEntities()
        {
            return c => c.Has<GuiComponent>();
        }
    }
}
