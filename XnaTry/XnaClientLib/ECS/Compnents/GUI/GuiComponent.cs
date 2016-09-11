using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents.GUI
{
    public abstract class GuiComponent : Component, IContentRequester
    {
        public virtual void Update(TimeSpan delta) { }
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void LoadContent(ContentManager content);
        public abstract int DrawOrder { get; }
        public abstract bool IsHud { get; }
        public Viewport? Viewport { get; set; } = null;
    }
}