using System;
using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using XnaClientLib.ECS.Compnents;

namespace XnaClientLib.ECS.Systems
{
    public class GuiComponentsSystem : XnaCommonLib.ECS.Systems.System
    {
        public SpriteBatch SpriteBatch { get; }
        public Camera Camera { get; }

        public GuiComponentsSystem(SpriteBatch spriteBatch, Camera camera, bool enabled = true) 
            : base(enabled)
        {
            SpriteBatch = spriteBatch;
            Camera = camera;
        }

        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            var allGuiComponents = entities.SelectMany(c => c.GetAllOf<GuiComponent>()).ToList();
            allGuiComponents.Sort((first, second) => first.DrawOrder().CompareTo(second.DrawOrder()));
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.CameraMatrix);
            foreach (var guiComponent in allGuiComponents)
            {
                guiComponent.Update(guiComponent.Container);
                guiComponent.Draw(SpriteBatch);
            }
            SpriteBatch.End();
        }

        public override Predicate<IComponentContainer> RelevantEntities()
        {
            return c => c.Has<GuiComponent>();
        }
    }
}
