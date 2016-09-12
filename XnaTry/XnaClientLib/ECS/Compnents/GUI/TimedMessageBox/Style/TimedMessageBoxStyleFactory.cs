using System;
using System.Collections.Generic;

namespace XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style
{
    public class TimedMessageBoxStyleFactory
    {
        #region Fields

        private readonly Dictionary<TimedMessageBoxStyle, Func<TimedMessageBoxItem, TimedMessageBoxItemStyleStrategy>> styles;

        #endregion Fields

        #region Constructor

        public TimedMessageBoxStyleFactory()
        {
            styles = new Dictionary<TimedMessageBoxStyle, Func<TimedMessageBoxItem, TimedMessageBoxItemStyleStrategy>>
            {
                [TimedMessageBoxStyle.None] = item => null,
                [TimedMessageBoxStyle.Fading] = item => new FadingStyleStrategy(item)
            };
        }

        #endregion Constructor

        public TimedMessageBoxItemStyleStrategy Create(TimedMessageBoxItem item, TimedMessageBoxStyle style)
        {
            return styles[style](item);
        }
    }
}