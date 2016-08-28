namespace XnaCommonLib.ECS.Components
{
    public class Lifespan : Component
    {
        public long RemainingTime { get; private set; }

        /// <summary>
        /// Initializes a timespan
        /// </summary>
        /// <param name="initialTimespan">Initial time span in milliseconds</param>
        public Lifespan(long initialTimespan)
        {
            RemainingTime = initialTimespan;
        }

        public void Update(long delta)
        {
            RemainingTime -= delta;
            if (RemainingTime <= 0)
                Container.Parent.Dispose();
        }
    }
}
