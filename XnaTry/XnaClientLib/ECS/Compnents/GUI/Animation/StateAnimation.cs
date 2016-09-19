using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace XnaClientLib.ECS.Compnents.GUI.Animation
{
    public class CharacterAnimation : StateAnimation<CharacterAnimationState>
    {
        #region Fields

        /// <summary>
        /// Indicates whether you can change to another animation if the current one is continuous
        /// </summary>
        private bool hasAnimationEnded;

        #endregion Fields

        #region Properties

        public override CharacterAnimationState CurrentState
        {
            get
            {
                return base.CurrentState;
            }
            set
            {
                if (!animations.ContainsKey(value))
                    throw new ArgumentOutOfRangeException("value", value, "Value of CurrentState must be in the animation states");

                if (currentState.Equals(value) && activeAnimation != null)
                    return;

                if (currentState.Continuous && !hasAnimationEnded)
                    return;

                currentState = value;
                activeAnimation = animations[currentState];
                hasAnimationEnded = false;
            }
        }

        #endregion Properties

        #region Constructor

        public CharacterAnimation(
            CharacterAnimationState defaultState,
            Dictionary<CharacterAnimationState, Animation> animations) : base(defaultState, animations)
        {
            hasAnimationEnded = true;
        }

        #endregion Constructor

        public override bool Update(long delta)
        {
            hasAnimationEnded = base.Update(delta);
            return hasAnimationEnded;
        }
    }

    /// <summary>
    /// Manages other animations by state
    /// </summary>
    /// <typeparam name="T">Type of the states of the animation. Can be an enum, an integer, a string, practically anything</typeparam>
    public class StateAnimation<T> : Animation
    {
        /// <summary>
        /// The current state of the animation
        /// </summary>
        protected T currentState;

        /// <summary>
        /// The animation states
        /// </summary>
        protected readonly Dictionary<T, Animation> animations;

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
        public virtual T CurrentState
        { 
            get
            {
                return currentState;
            }
            set
            {
                if (!animations.ContainsKey(value))
                    throw new ArgumentOutOfRangeException("value", value, "Value of CurrentState must be in the animation states");

                if (currentState.Equals(value) && activeAnimation != null)
                    return;

                currentState = value;
                activeAnimation = animations[currentState];
            }
        }

        /// <summary>
        /// The animation currently active
        /// </summary>
        protected Animation activeAnimation;

        /// <summary>
        /// Initialize the state animation
        /// </summary>
        /// <param name="defaultState">The default state of the animation, must be one of the states of the animations</param>
        /// <param name="animations">The animations managed by the state animation</param>
        public StateAnimation(T defaultState, Dictionary<T, Animation> animations)
        {
            if (!animations.ContainsKey(defaultState))
                throw new ArgumentOutOfRangeException("defaultState", defaultState, "The default state must be one of the states of the animation");

            this.animations = animations;
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
            activeAnimation.Disable();
        }
        
        public override bool Update(long delta)
        {
            if (!Enabled)
            {
                Disable();
                return true;
            }

            activeAnimation.Enabled = true;
            return activeAnimation.Update(delta);
        }
    }
}