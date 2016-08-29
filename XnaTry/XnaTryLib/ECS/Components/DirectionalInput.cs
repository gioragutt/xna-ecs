using Microsoft.Xna.Framework;
using UtilsLib.Consts;

namespace XnaCommonLib.ECS.Components
{
    /// <summary>
    /// Base class for deriving directional movement input from an outer source
    /// </summary>
    public abstract class DirectionalInput : Component, IUpdatable<DirectionalInput>
    {
        public void Update(DirectionalInput instance)
        {
            Horizontal = instance.Horizontal;
            Vertical = instance.Vertical;
        }

        private static float ClampInput(float input)
        {
            return MathHelper.Clamp(input, Constants.Game.FullNegativeInput, Constants.Game.FullPositiveInput);
        }

        #region Properties

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
            set
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
            set
            {
                vertical = ClampInput(value);
            }
        }

        #endregion Properties

        public override string ToString()
        {
            return string.Format("Horizontal: ( {0} ) Vertical: ( {1} )", Horizontal.ToString("0.00"), Vertical.ToString("0.00"));
        }

        public abstract void Update(long delta);
    }
}
