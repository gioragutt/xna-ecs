using ECS.Interfaces;
using XnaClientLib.ECS.Compnents.GUI.Animation;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Linkers
{
    public class MovementToAnimationLinker : Linker<IComponentContainer, StateAnimation<AnimationState>>
    {
        private DirectionalInput Input { get; }
        private PlayerAttributes Attributes { get; }
        public MovementToAnimationLinker(IComponentContainer first, StateAnimation<AnimationState> second) : base(first, second)
        {
            Input = First.Get<DirectionalInput>();
            Attributes = First.Get<PlayerAttributes>();
        }

        public override void Link()
        {
            if (!Input.IsMoving() || Attributes.IsDead)
            {
                Second.Enabled = false;
                return;
            }

            Second.Enabled = true;
            var animationSpeed = 0f;
            var direction = UpdateAnimation(ref animationSpeed);

            Second.CurrentState = direction;
            Second.AnimationSpeed = animationSpeed;
        }

        private AnimationState UpdateAnimation(ref float animationSpeed)
        {
            var state = Second.DefaultState;

            if (Input.Vertical > 0)
            {
                state = AnimationState.Get(AnimationType.Walk, AnimationDirection.Down);
                animationSpeed = Input.Vertical;
            }
            else if (Input.Vertical < 0)
            {
                state = AnimationState.Get(AnimationType.Walk, AnimationDirection.Up);
                animationSpeed = Input.Vertical;
            }
            if (Input.Horizontal > 0)
            {
                state = AnimationState.Get(AnimationType.Walk, AnimationDirection.Right);
                animationSpeed = Input.Horizontal;
            }
            else if (Input.Horizontal < 0)
            {
                state = AnimationState.Get(AnimationType.Walk, AnimationDirection.Left);
                animationSpeed = Input.Horizontal;
            }
            return state;
        }
    }
}