using Microsoft.Xna.Framework;

namespace XnaTryLib.ECS.Components
{
    /// <summary>
    /// Base class for deriving directional movement input from an outer source
    /// </summary>
    public abstract class DirectionalInput : Component
    {
        private static float ClampInput(float input)
        {
            return MathHelper.Clamp(input, Constants.FullNegativeInput, Constants.FullPositiveInput);
        }

        private float horizontal;
        private float vertical;

        /// <summary>
        /// Indicates the input from the horizontal direction
        /// </summary>
        /// <remarks>
        /// Values between Constants.FullPositiveInput to Constants.FullNegativeInput.
        /// </remarks>
        public float Horizontal
        {
            get
            {
                return horizontal;
            }
            protected set
            {
                horizontal = ClampInput(value);
            }
        }

        /// <summary>
        /// Indicates the input from the veritcal direction
        /// </summary>
        /// <remarks>
        /// Values between Constants.FullPositiveInput to Constants.FullNegativeInput.
        /// </remarks>
        public float Vertical
        {
            get
            {
                return vertical;
            }
            protected set
            {
                vertical = ClampInput(value);
            }
        }

        public override string ToString()
        {
            return string.Format("Horizontal: ( {0} ) Vertical: ( {1} )", Horizontal.ToString("0.00"), Vertical.ToString("0.00"));
        }

        public abstract void Update(long delta);
    }
}
