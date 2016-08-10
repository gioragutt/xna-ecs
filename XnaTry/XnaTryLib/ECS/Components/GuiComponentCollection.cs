using System.Collections.Generic;
using ECS.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTryLib.ECS.Components
{
    public class GuiComponentCollection : GuiComponent
    {
        public List<GuiComponent> Collection { get; set; }

        public override void Update(IComponentContainer entity)
        {
            Collection.ForEach(c => c.Update(entity)); 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Collection.ForEach(c => c.Draw(spriteBatch));
        }

        public override void LoadContent(ContentManager content)
        {
            Collection.ForEach(c => c.LoadContent(content));
        }
    }
}
