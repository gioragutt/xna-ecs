using Microsoft.Xna.Framework.Input;

namespace XnaTryLib.ECS.Components
{
    public struct KeyboardSettings
    {
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Up { get; set; }
        public Keys Down { get; set; }

        public KeyboardSettings(Keys left = Keys.Left, Keys right = Keys.Right, Keys up = Keys.Up, Keys down = Keys.Down)
        {
            Left = left;
            Right = right;
            Up = up;
            Down = down;
        }
    }

    public class KeyboardDirectionalInput : DirectionalInput
    {
        private KeyboardSettings Settings { get; }

        private static readonly KeyboardSettings DefaultSettings = new KeyboardSettings
        {
            Down = Keys.Down,
            Up = Keys.Up,
            Right = Keys.Right,
            Left = Keys.Left
        };

        /// <summary>
        /// Initializes the input component with the key settings
        /// </summary>
        /// <param name="settings">Settings; default (arrow keys) if null specified</param>
        public KeyboardDirectionalInput(KeyboardSettings? settings = null)
        {
            Settings = settings ?? DefaultSettings;
        }

        public override void Update(long delta)
        {
            Vertical = Horizontal = 0;
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Settings.Down))
                Vertical += Constants.FullPositiveInput;
            if (keyboard.IsKeyDown(Settings.Up))
                Vertical += Constants.FullNegativeInput;
            if (keyboard.IsKeyDown(Settings.Right))
                Horizontal += Constants.FullPositiveInput;
            if (keyboard.IsKeyDown(Settings.Left))
                Horizontal += Constants.FullNegativeInput;
        }
    }
}
