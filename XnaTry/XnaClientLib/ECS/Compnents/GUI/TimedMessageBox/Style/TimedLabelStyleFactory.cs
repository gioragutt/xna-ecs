using System;
using System.Collections.Generic;

namespace XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style
{
    public class TimedLabelStyleFactory
    {
        #region Fields

        private readonly Dictionary<TimedMessageBoxStyle, Func<TimedLabel, TimedLabelStyleStrategy>> styles;

        #endregion Fields

        #region Constructor

        public TimedLabelStyleFactory()
        {
            styles = new Dictionary<TimedMessageBoxStyle, Func<TimedLabel, TimedLabelStyleStrategy>>
            {
                [TimedMessageBoxStyle.None] = item => null,
                [TimedMessageBoxStyle.Fading] = item => new FadingStyleStrategy(item)
            };
        }

        #endregion Constructor

        public TimedLabelStyleStrategy Create(TimedLabel item, TimedMessageBoxStyle style)
        {
            return styles[style](item);
        }
    }
}