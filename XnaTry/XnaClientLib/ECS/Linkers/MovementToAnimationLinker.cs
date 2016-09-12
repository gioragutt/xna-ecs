using ECS.Interfaces;
using XnaClientLib.ECS.Compnents.GUI.Animation;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Linkers
{
    public class MovementToAnimationLinker : Linker<IComponentContainer, StateAnimation<MovementDirection>>
    {
        private DirectionalInput Input { get; }
        private PlayerAttributes Attributes { get; }
        public MovementToAnimationLinker(IComponentContainer first, StateAnimation<MovementDirection> second) : base(first, second)
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
            var direction = Second.DefaultState;
            var animationSpeed = 0f;
            direction = UpdateAnimation(direction, ref animationSpeed);

            Second.CurrentState = direction;
            Second.AnimationSpeed = animationSpeed;
        }

        private MovementDirection UpdateAnimation(MovementDirection direction, ref float animationSpeed)
        {
            if (Input.Vertical > 0)
            {
                direction = MovementDirection.Down;
                animationSpeed = Input.Vertical;
            }
            else if (Input.Vertical < 0)
            {
                direction = MovementDirection.Up;
                animationSpeed = Input.Vertical;
            }
            if (Input.Horizontal > 0)
            {
                direction = MovementDirection.Right;
                animationSpeed = Input.Horizontal;
            }
            else if (Input.Horizontal < 0)
            {
                direction = MovementDirection.Left;
                animationSpeed = Input.Horizontal;
            }
            return direction;
        }
    }
}