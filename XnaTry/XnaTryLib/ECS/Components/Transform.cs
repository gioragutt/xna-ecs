using Microsoft.Xna.Framework;
using UtilsLib.Consts;

namespace XnaCommonLib.ECS.Components
{
    public class Transform : Component, IUpdatable<Transform>
    {
        public void Update(Transform instance)
        {
            Position = instance.Position;
            Rotation = instance.Rotation;
            Scale = instance.Scale;
        }

        #region Fields

        private float scale;
        private float rotation;

        #endregion

        #region Properties

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
                rotation = (value + Constants.Game.MaxRotation) % Constants.Game.MaxRotation;
            }
        }

        /// <summary>
        /// The position of the entity
        /// </summary>
        public Vector2 Position { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new Transform Component
        /// </summary>
        /// <param name="position">The position of the transform, defaulting at Microsoft.Xna.Framework.Vector2.Zero</param>
        /// <param name="scale">The scale of the transform, defaulting at 1</param>
        /// <param name="rotation">The rotation of the transform, defaulting at 0</param>
        /// <param name="enabled">The enabled status of the transform, defaulting to true</param>
        public Transform(Vector2 position, float scale = 1f, float rotation = 0f, bool enabled = true)
            : base(enabled)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        /// <summary>
        /// Initializes a default transform
        /// </summary>
        public Transform() : this(Vector2.Zero)
        {
        }

        #endregion Constructor

        public override string ToString()
        {
            const string format = "{0}, {1}";
            return string.Format(format,
                Position.X.ToString("0000"), 
                Position.Y.ToString("0000"));
        }

        #region Transform API

        /// <summary>
        /// Move the transform position by the given vector
        /// </summary>
        /// <param name="vector">The vector to move by</param>
        public void MoveBy(Vector2 vector)
        {
            Position = Vector2.Add(Position, vector);
        }

        /// <summary>
        /// Rotates the transform towards the given point
        /// </summary>
        /// <param name="point">The point to rotate to</param>
        public void RotateTo(Vector2 point)
        {
            var diff = Vector2.Subtract(point, Position);
            Rotation = diff.ToRadians();
        }

        #endregion
    }
}
