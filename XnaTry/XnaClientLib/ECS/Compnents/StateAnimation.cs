using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace XnaClientLib.ECS.Compnents
{
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

        public StateAnimation(Sprite sprite, long msPerFrame, T defaultState, Dictionary<T, Animation> animations) 
            : base(sprite, msPerFrame)
        {
            Animations = animations;
            DefaultState = defaultState;

            if (animations.ContainsKey(DefaultState))
                CurrentState = DefaultState;
            else if (animations.Count > 0)
                CurrentState = animations.Keys.First();
        }

        public StateAnimation(Sprite sprite, long msPerFrame, T initialState)
            : this(sprite, msPerFrame, initialState, new Dictionary<T, Animation>())
        {
        }

        public void AddState(T id, Animation animation)
        {
            if (Animations.ContainsKey(id))
                return;

            Animations.Add(id, animation);

            if (id.Equals(DefaultState))
                CurrentState = id;
        }

        public override void LoadContent(ContentManager content)
        {
            ActiveAnimation.LoadContent(content);
        }

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