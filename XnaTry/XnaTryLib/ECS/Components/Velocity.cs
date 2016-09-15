using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace XnaCommonLib.ECS.Components
{
    public class Velocity : Component, IUpdatable<Velocity>
    {
        #region Properties

        public Vector2 VelocityVector { get; set; }

        [JsonIgnore]
        public float X => VelocityVector.X;

        [JsonIgnore]
        public float Y => VelocityVector.Y;

        #endregion Properties

        #region Constructor

        public Velocity(Vector2 velocityVector)
        {
            VelocityVector = velocityVector;
        }

        #endregion Constructor

        #region IUpdatable<Velocity>

        public void Update(Velocity instance)
        {
            VelocityVector = instance.VelocityVector;
        }

        #endregion IUpdatable<Velocity>

        #region Helper Methods

        public static Vector2 operator *(Velocity vel, DirectionalInput input)
        {
            return new Vector2(vel.X * input.Horizontal, vel.Y * input.Vertical);
        }

        #endregion
    }
}