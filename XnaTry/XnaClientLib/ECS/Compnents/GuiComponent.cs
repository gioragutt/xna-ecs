using ECS.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public abstract class GuiComponent : Component, IContentRequester
    {
        public virtual void Update(IComponentContainer entity) { }
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void LoadContent(ContentManager content);
    }
}