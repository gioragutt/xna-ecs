using System;
using Microsoft.Xna.Framework;

namespace XnaTryLib.ECS.Components
{
    public class Transform : BaseComponent
    {
        private float scale;
        private float rotation;

        /// <summary>
        /// The scale of the entity
        /// </summary>
        /// <remarks>
        /// Scale is multiplicative. 1F is normal size, 2F is twice as big, 0.5F is twice as small
        /// </remarks>
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value <= 0 ? 0 : value;
            }
        }

        /// <summary>
        /// Angle of rotation in radians
        /// </summary>
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = (value + Constants.MaxRotation) % Constants.MaxRotation;
            }
        }

        /// <summary>
        /// The position of the entity
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Initializes a new Transform Component
        /// </summary>
        /// <param name="position">The position of the transform, defaulting at Microsoft.Xna.Framework.Vector2.Zero</param>
        /// <param name="scale">The scale of the transform, defaulting at 1</param>
        /// <param name="rotation">The rotation of the transform, defaulting at 0</param>
        /// <param name="enabled">The enabled status of the transform, defaulting to true</param>
        public Transform(
            Vector2 position,
            float scale = Constants.DefaultScale,
            float rotation = Constants.DefaultRotation,
            bool enabled = true) : base(enabled)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        /// <summary>
        /// Initializes a default transform
        /// </summary>
        public Transform() : this(Vector2.Zero) { }

        public override string ToString()
        {
            const string format = "P ( {0}, {1} ) R ( {2} ) S ( {3} )";
            return string.Format(format,
                Position.X.ToString("0000"), 
                Position.Y.ToString("0000"), 
                MathHelper.ToDegrees(Rotation).ToString("000"),
                Scale);
        }

        /// <summary>
        /// Move the transform position by the given vector
        /// </summary>
        /// <param name="vector">The vector to move by</param>
        public void MoveBy(Vector2 vector) { Position = Vector2.Add(Position, vector); }

        #region Rotation Methods

        /// Currently not used, as rotation part was initially in the MovementSystem
        /// And it should be in a component of it's own

        public void RotateTo(Vector2 point)
        {
            var angle = AngleBetween(Position, point);
            Rotation = MathHelper.ToRadians((float)angle);
        }

        public static double AngleBetween(Vector2 first, Vector2 second)
        {
            double sin = first.X * second.Y - second.X * first.Y;
            double cos = first.X * second.X + first.Y * second.Y;

            return Math.Atan2(sin, cos) * (180 / Math.PI);
        }

        #endregion
    }
}
