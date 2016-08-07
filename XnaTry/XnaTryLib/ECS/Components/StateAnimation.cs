using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace XnaTryLib.ECS.Components
{
    public class StateBasedAnimation<T> : Animation
    {
        private Dictionary<T, Animation> States { get; }

        public T DefaultState { get; set; }

        public T CurrentState
        {
            set
            {
                if (!States.ContainsKey(value))
                    throw new ArgumentOutOfRangeException("value", value, "Value of CurrentState must added to the animation states");

                CurrentAnimation = States[value];
            }
        }

        private Animation CurrentAnimation { get; set; }

        public StateBasedAnimation(Sprite sprite, long msPerFrame, T defaultState, Dictionary<T, Animation> states) 
            : base(sprite, msPerFrame)
        {
            States = states;
            DefaultState = defaultState;
        }

        public StateBasedAnimation(Sprite sprite, long msPerFrame, T initialState)
            : this(sprite, msPerFrame, initialState, new Dictionary<T, Animation>())
        {
        }

        public void AddState(T id, Animation animation)
        {
            if (States.ContainsKey(id))
                return;

            States.Add(id, animation);

            if (id.Equals(DefaultState))
                CurrentState = id;
        }

        public override void LoadContent(ContentManager content)
        {
            CurrentAnimation.LoadContent(content);
        }

        public override void Disable()
        {
            CurrentState = DefaultState;
            CurrentAnimation.Disable();
        }
        
        public override void Update(long delta)
        {
            if (!Enabled)
            {
                Disable();
                return;
            }

            CurrentAnimation.Enabled = true;
            CurrentAnimation.Update(delta);
        }
    }
}