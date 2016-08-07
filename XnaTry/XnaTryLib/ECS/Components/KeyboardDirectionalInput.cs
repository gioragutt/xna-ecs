using Microsoft.Xna.Framework.Input;

namespace XnaTryLib.ECS.Components
{
    public struct KeyboardLayoutOptions
    {
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Up { get; set; }
        public Keys Down { get; set; }

        public KeyboardLayoutOptions(Keys left = Keys.Left, Keys right = Keys.Right, Keys up = Keys.Up, Keys down = Keys.Down)
        {
            Left = left;
            Right = right;
            Up = up;
            Down = down;
        }
    }

    public class KeyboardDirectionalInput : DirectionalInput
    {
        private KeyboardLayoutOptions LayoutOptions { get; }

        public static readonly KeyboardLayoutOptions DefaultLayoutOptions = new KeyboardLayoutOptions
        {
            Down = Keys.Down,
            Up = Keys.Up,
            Right = Keys.Right,
            Left = Keys.Left
        };

        /// <summary>
        /// Initializes the input component with the key LayoutOptions
        /// </summary>
        /// <param name="layoutOptions">LayoutOptions; default (arrow keys) if null specified</param>
        public KeyboardDirectionalInput(KeyboardLayoutOptions? layoutOptions = null)
        {
            LayoutOptions = layoutOptions ?? DefaultLayoutOptions;
        }

        public override void Update(long delta)
        {
            Vertical = Horizontal = 0;
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(LayoutOptions.Down))
                Vertical += Constants.FullPositiveInput;
            if (keyboard.IsKeyDown(LayoutOptions.Up))
                Vertical += Constants.FullNegativeInput;
            if (keyboard.IsKeyDown(LayoutOptions.Right))
                Horizontal += Constants.FullPositiveInput;
            if (keyboard.IsKeyDown(LayoutOptions.Left))
                Horizontal += Constants.FullNegativeInput;
        }
    }
}
