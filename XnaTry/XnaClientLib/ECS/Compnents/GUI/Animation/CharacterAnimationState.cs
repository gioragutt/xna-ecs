using System;

namespace XnaClientLib.ECS.Compnents.GUI.Animation
{
    public struct CharacterAnimationState : IEquatable<CharacterAnimationState>
    {
        #region Properties

        public AnimationType Type { get; }
        public AnimationDirection Direction { get; }
        public bool Continuous { get; }

        #endregion Properties

        #region Constructor

        private CharacterAnimationState(AnimationType type, AnimationDirection direction, bool continuous)
        {
            Type = type;
            Direction = direction;
            Continuous = continuous;
        }

        #endregion

        #region Factory Method

        public static CharacterAnimationState Get(AnimationType type, AnimationDirection direction, bool continuous = false)
        {
            return new CharacterAnimationState(type, direction, continuous);
        }

        #endregion

        #region Equality Methods

        public bool Equals(CharacterAnimationState other)
        {
            return Type == other.Type && Direction == other.Direction;
        }

        public static bool operator ==(CharacterAnimationState first, CharacterAnimationState second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(CharacterAnimationState first, CharacterAnimationState second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is CharacterAnimationState && Equals((CharacterAnimationState)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Type * 397) ^ (int)Direction;
            }
        }
        #endregion Equality Methods
    }
}