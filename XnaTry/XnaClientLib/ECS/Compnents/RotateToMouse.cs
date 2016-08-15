using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public class RotateToMouse : Component
    {
        public Vector2 MousePosition => Mouse.GetState().GetPosition();
    }
}
