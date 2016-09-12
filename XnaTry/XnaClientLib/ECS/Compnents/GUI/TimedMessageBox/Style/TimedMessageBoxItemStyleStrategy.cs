namespace XnaClientLib.ECS.Compnents.GUI.TimedMessageBox.Style
{
    public abstract class TimedMessageBoxItemStyleStrategy
    {
        #region Fields

        protected readonly TimedMessageBoxItem item;

        #endregion Fields

        #region Constructor

        protected TimedMessageBoxItemStyleStrategy(TimedMessageBoxItem styledItem)
        {
            item = styledItem;
        }

        #endregion Constructor

        #region API

        public abstract void Update();

        #endregion
    }
}