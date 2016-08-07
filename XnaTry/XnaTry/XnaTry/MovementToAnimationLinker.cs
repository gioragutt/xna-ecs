using XnaTryLib;
using XnaTryLib.ECS.Components;

namespace XnaTry
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

            if (First.Vertical > 0)
                direction = MovementDirection.Down;
            else if (First.Vertical < 0)
                direction = MovementDirection.Up;
            if (First.Horizontal > 0)
                direction = MovementDirection.Right;
            else if (First.Horizontal < 0)
                direction = MovementDirection.Left;

            Second.CurrentState = direction;
        }

        public MovementToAnimationLinker(DirectionalInput first, StateAnimation<MovementDirection> second) 
            : base(first, second)
        {

        }
    }
}