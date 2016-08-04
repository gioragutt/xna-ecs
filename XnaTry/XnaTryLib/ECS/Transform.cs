using Microsoft.Xna.Framework;

namespace XnaTryLib.ECS
{
    public class Transform : BaseComponent
    {
        /// <summary>
        /// The scale of the entity
        /// </summary>
        /// <remarks>
        /// Scale is multiplicative. 1F is normal size, 2F is twice as big, 0.5F is twice as small
        /// </remarks>
        public float Scale { get; set; }

        /// <summary>
        /// Angle of rotation in radians
        /// </summary>
        public float Rotation { get; set; }

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
        public Transform()
            : this(Vector2.Zero)
        {
        }
    }
}
