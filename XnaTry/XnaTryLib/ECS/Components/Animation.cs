using Microsoft.Xna.Framework.Content;

namespace XnaTryLib.ECS.Components
{
    public abstract class Animation : Component
    {
        /// <summary>
        /// The sprite the animation is going to animate
        /// </summary>
        public Sprite Sprite { get; set; }

        /// <summary>
        /// The Frames Per Second rate
        /// </summary>
        public long MsPerFrame { get; set; }

        /// <summary>
        /// Initializes variables of Animation
        /// </summary>
        /// <param name="sprite">Sprite to update</param>
        /// <param name="msPerFrame">Milliseconds between frame updates</param>
        /// <exception cref="System.ArgumentNullException">sprite it null</exception>
        protected Animation(Sprite sprite, long msPerFrame)
        {
            Util.AssertArgumentNotNull(sprite, "sprite");

            Sprite = sprite;
            MsPerFrame = msPerFrame;
            Enabled = false;
        }

        /// <summary>
        /// Loads the required resources for the animation, if needed
        /// </summary>
        /// <param name="content">Content manager to use</param>
        public abstract void LoadContent(ContentManager content);

        /// <summary>
        /// Use to disable the usage of the animation, returning it to it's default state
        /// </summary>
        public abstract void Disable();

        /// <summary>
        /// Update the state of the animation
        /// </summary>
        /// <param name="delta">Time since last update</param>
        public abstract void Update(long delta);
    }
}
