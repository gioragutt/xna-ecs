using System.IO;
using Microsoft.Xna.Framework;

namespace XnaCommonLib.ECS.Components
{
    public class Velocity : Component, ISharedComponent
    {
        public Vector2 VelocityVector { get; set; }
        public float X => VelocityVector.X;
        public float Y => VelocityVector.Y;

        public Velocity(Vector2 velocityVector)
        {
            VelocityVector = velocityVector;
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(VelocityVector.X);
            writer.Write(VelocityVector.Y);
        }

        public void Read(BinaryReader reader)
        {
            VelocityVector = new Vector2
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle()
            };
        }
    }
}