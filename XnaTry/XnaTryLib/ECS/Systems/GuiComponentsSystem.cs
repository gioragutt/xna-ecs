using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaTryLib.ECS.Components;

namespace XnaTryLib.ECS.Systems
{
    public class GuiComponentsSystem : System
    {
        public SpriteBatch SpriteBatch { get; set;  }

        public override void Update(ICollection<IComponentContainer> entities, long delta)
        {
            var allGuiComponents = entities.SelectMany(c => c.GetAllOf<GuiComponent>());
            SpriteBatch.Begin();
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
