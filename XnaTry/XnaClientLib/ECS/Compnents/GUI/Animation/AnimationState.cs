using System;

namespace XnaClientLib.ECS.Compnents.GUI.Animation
{
    public struct AnimationState : IEquatable<AnimationState>
    {
        #region Properties

        public AnimationType Type { get; set; }
        public AnimationDirection Direction { get; set; }

        #endregion Properties

        #region Constructor

        private AnimationState(AnimationType type, AnimationDirection direction)
        {
            Type = type;
            Direction = direction;
        }

        #endregion

        #region Factory Method

        public static AnimationState Get(AnimationType type, AnimationDirection direction)
        {
            return new AnimationState(type, direction);
        }

        #endregion

        #region IEquatable<AnimationState> Methods

        public bool Equals(AnimationState other)
        {
            return Type == other.Type && Direction == other.Direction;
        }

        #endregion IEquatable<AnimationState> Methods
    }
}