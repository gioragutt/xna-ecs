using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public class GamepadDirectionalInput : DirectionalInput
    {
        public override void Update(long delta)
        {
            var currentState = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            Horizontal = currentState.X;
            Vertical = currentState.Y;
        }
    }
}
