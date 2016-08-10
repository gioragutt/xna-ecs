using ECS.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using XnaTryLib.ECS.Components;

namespace XnaTryLib.ECS.Components
{
    public abstract class GuiComponent : Component
    {
        public abstract void Update(IComponentContainer entity);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}