using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style;

namespace XnaClientLib.ECS.Compnents.GUI.TimedMessageBox
{
    public class TimedLabel : Label
    {
        #region Fields

        public TimeSpan MaxTime { get; }

        #endregion

        #region Properties

        public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;

        public Color OriginalColor { get; }
        public TimedLabelStyleStrategy Style { get; set; }

        public bool IsExpired => Elapsed >= MaxTime;

        #endregion

        #region Constructor

        public TimedLabel(object value, TimeSpan maxExpiredTime, Color itemColor, SpriteFont font)
            : base(value, itemColor, font, Vector2.Zero)
        {
            MaxTime = maxExpiredTime;
            OriginalColor = itemColor;
        }

        #endregion
    }
}