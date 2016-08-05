using Microsoft.Xna.Framework;

namespace XnaTryLib.ECS.Components
{
    public class Velocity : BaseComponent
    {
        public Vector2 VelocityVector { get; set; }
        public float X => VelocityVector.X;
        public float Y => VelocityVector.Y;
    }
}