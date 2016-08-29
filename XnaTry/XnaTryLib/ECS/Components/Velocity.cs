using Microsoft.Xna.Framework;

namespace XnaCommonLib.ECS.Components
{
    public class Velocity : Component, IUpdatable<Velocity>
    {
        public Vector2 VelocityVector { get; set; }
        public float X => VelocityVector.X;
        public float Y => VelocityVector.Y;

        public Velocity(Vector2 velocityVector)
        {
            VelocityVector = velocityVector;
        }

        public void Update(Velocity instance)
        {
            VelocityVector = instance.VelocityVector;
        }

        public static Vector2 operator *(Velocity vel, DirectionalInput input)
        {
            return new Vector2(vel.X * input.Horizontal, vel.Y * input.Vertical);
        }
    }
}