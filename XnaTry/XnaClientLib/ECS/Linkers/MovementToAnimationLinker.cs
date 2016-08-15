using XnaClientLib.ECS.Compnents;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Linkers
{
    public class MovementToAnimationLinker : Linker<DirectionalInput, StateAnimation<MovementDirection>>
    {
        public override void Link()
        {
            if (!First.IsMoving())
            {
                if (!Second.Enabled)
                    return;

                Second.Enabled = false;
                return;
            }

            Second.Enabled = true;
            var direction = Second.DefaultState;
            var animationSpeed = 0f;
            direction = UpdateAnimation(direction, ref animationSpeed);

            Second.CurrentState = direction;
            Second.AnimationSpeed = animationSpeed;
        }

        private MovementDirection UpdateAnimation(MovementDirection direction, ref float animationSpeed)
        {
            if (First.Vertical > 0)
            {
                direction = MovementDirection.Down;
                animationSpeed = First.Vertical;
            }
            else if (First.Vertical < 0)
            {
                direction = MovementDirection.Up;
                animationSpeed = First.Vertical;
            }
            if (First.Horizontal > 0)
            {
                direction = MovementDirection.Right;
                animationSpeed = First.Horizontal;
            }
            else if (First.Horizontal < 0)
            {
                direction = MovementDirection.Left;
                animationSpeed = First.Horizontal;
            }
            return direction;
        }

        public MovementToAnimationLinker(DirectionalInput first, StateAnimation<MovementDirection> second) 
            : base(first, second)
        {

        }
    }
}