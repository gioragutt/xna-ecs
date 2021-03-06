using Microsoft.Xna.Framework;

namespace XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style
{
    public class FadingStyleStrategy : TimedLabelStyleStrategy
    {
        #region Constructor

        public FadingStyleStrategy(TimedLabel styledItem) 
            : base(styledItem)
        {
        }

        #endregion

        #region API

        public override void Update()
        {
            item.Color = item.OriginalColor *
                         MathHelper.Lerp(1, 0, (float) (item.Elapsed.TotalMilliseconds / item.MaxTime.TotalMilliseconds));
        }

        #endregion API
    }
}