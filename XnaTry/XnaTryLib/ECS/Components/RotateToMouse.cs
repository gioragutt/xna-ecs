using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XnaTryLib.ECS.Components
{
    public class RotateToMouse : BaseComponent
    {
        public Vector2 MousePosition => Mouse.GetState().GetPosition();
    }
}
