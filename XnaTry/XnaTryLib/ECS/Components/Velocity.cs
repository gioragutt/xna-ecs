using Microsoft.Xna.Framework;

namespace XnaCommonLib.ECS.Components
{
    public class Velocity : Component
    {
        public Vector2 VelocityVector { get; set; }
        public float X => VelocityVector.X;
        public float Y => VelocityVector.Y;

        public Velocity(Vector2 velocityVector)
        {
            VelocityVector = velocityVector;
        }
    }
}