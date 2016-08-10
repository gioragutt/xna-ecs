using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaTryLib.ECS.Components
{
    public enum LabelPlacement
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public class Label : Component
    {
        public Label(string text = "", LabelPlacement placement = LabelPlacement.TopCenter, Color? color = null, SpriteFont font = null , bool enabled = true)
            : base(enabled)
        {
            Text = text;
            Placement = placement;
            Font = font;
            Color = color;
        }

        public string Text { get; set; }
        public LabelPlacement Placement { get; set; }
        public SpriteFont Font { get; private set; }
        public Color? Color { get; set; }
    }
}
