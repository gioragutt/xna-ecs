using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace XnaClientLib.ECS.Compnents.GUI
{
    /// <summary>
    /// Manages other animations by state
    /// </summary>
    /// <typeparam name="T">Type of the states of the animation. Can be an enum, an integer, a string, practically anything</typeparam>
    public class StateAnimation<T> : Animation
    {
        private T currentState;

        /// <summary>
        /// The animation states
        /// </summary>
        private Dictionary<T, Animation> Animations { get; }

        /// <summary>
        /// The default state to return to when disable
        /// </summary>
        public T DefaultState { get; set; }

        /// <summary>
        /// The current state of the animation
        /// </summary>
        /// <remarks>
        /// Setting it updates the animation state
        /// </remarks>
        public T CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                if (!Animations.ContainsKey(value))
                    throw new ArgumentOutOfRangeException("value", value, "Value of CurrentState must be in the animation states");

                if (currentState.Equals(value) && ActiveAnimation != null)
                    return;

                currentState = value;
                ActiveAnimation = Animations[currentState];
            }
        }

        /// <summary>
        /// The animation currently active
        /// </summary>
        private Animation ActiveAnimation { get; set; }

        /// <summary>
        /// Initialize the state animation
        /// </summary>
        /// <param name="defaultState">The default state of the animation, must be one of the states of the animations</param>
        /// <param name="animations">The animations managed by the state animation</param>
        public StateAnimation(T defaultState, Dictionary<T, Animation> animations)
        {
            if (!animations.ContainsKey(DefaultState))
                throw new ArgumentOutOfRangeException("defaultState", defaultState, "The default state must be one of the states of the animation");

            Animations = animations;
            DefaultState = defaultState;
            CurrentState = DefaultState;
        }

        public override void LoadContent(ContentManager content)
        {
            // No need to load any content, since the state animation doesn't store any content, but
            // Just manages other animations
        }

        /// <summary>
        /// When you disable the animation, you want to get back to the default state and
        /// Disable the animation of the default state
        /// </summary>
        public override void Disable()
        {
            CurrentState = DefaultState;
            ActiveAnimation.Disable();
        }
        
        public override void Update(long delta)
        {
            if (!Enabled)
            {
                Disable();
                return;
            }

            ActiveAnimation.Enabled = true;
            ActiveAnimation.Update(delta);
        }
    }
}