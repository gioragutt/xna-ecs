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

        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            var allGuiComponents = entities.SelectMany(c => c.GetAllOf<GuiComponent>());
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.CameraMatrix);
            foreach (var guiComponent in allGuiComponents)
            {
                guiComponent.Update(guiComponent.Container);
                guiComponent.Draw(SpriteBatch);
            }
            SpriteBatch.End();
        }

        public override ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<GuiComponent>()).ToList();
        }
    }
}
