namespace XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style
{
    public abstract class TimedLabelStyleStrategy
    {
        #region Fields

        protected readonly TimedLabel item;

        #endregion Fields

        #region Constructor

        protected TimedLabelStyleStrategy(TimedLabel styledItem)
        {
            item = styledItem;
        }

        #endregion Constructor

        #region API

        public abstract void Update();

        #endregion
    }
}