using ECS.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTryLib.ECS.Components
{
    public abstract class GuiComponent : Component, IContentRequester
    {
        public virtual void Update(IComponentContainer entity) { }
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void LoadContent(ContentManager content);
    }
}