using System;
using Microsoft.Xna.Framework;
using XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style;

namespace XnaClientLib.ECS.Compnents.GUI.TimedMessageBox
{
    public class TimedMessageBoxItem
    {
        #region Fields

        public TimeSpan MaxTime { get; }

        #endregion

        #region Properties

        public TimeSpan Elapsed { get; set; } = TimeSpan.Zero;
        public object Value { get; set; }

        public Color OriginalColor { get; }
        public Color Color { set; get; }
        public TimedMessageBoxItemStyleStrategy Style { get; set; }

        public bool IsExpired => Elapsed >= MaxTime;

        #endregion

        #region Constructor

        public TimedMessageBoxItem(object value, TimeSpan maxExpiredTime, Color itemColor)
        {
            Value = value;
            MaxTime = maxExpiredTime;
            Color = itemColor;
            OriginalColor = itemColor;
        }

        #endregion
    }
}