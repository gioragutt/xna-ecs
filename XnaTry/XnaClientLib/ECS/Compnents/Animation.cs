using Microsoft.Xna.Framework.Content;
using UtilsLib.Utility;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public abstract class Animation : Component, IContentRequester
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
        /// The Frames Per Second rate
        /// </summary>
        public float AnimationSpeed { get; set; }

        /// <summary>
        /// Initializes variables of Animation
        /// </summary>
        /// <param name="sprite">Sprite to update</param>
        /// <param name="msPerFrame">Milliseconds between frame updates</param>
        /// <exception cref="System.ArgumentNullException">sprite it null</exception>
        protected Animation(Sprite sprite, long msPerFrame)
        {
            Utils.AssertArgumentNotNull(sprite, "sprite");

            Sprite = sprite;
            MsPerFrame = msPerFrame;
            Enabled = false;
            AnimationSpeed = 1f;
        }

        /// <summary>
        /// Initializes variables of Animation
        /// </summary>
        protected Animation()
        {
            Sprite = null;
            MsPerFrame = 0;
            Enabled = false;
            AnimationSpeed = 1f;
        }

        /// <summary>
        /// Loads the required resources for the animation
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
