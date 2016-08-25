using ECS.Interfaces;
using Microsoft.Xna.Framework;
using XnaCommonLib;
using XnaCommonLib.ECS.Components;

namespace XnaClientLib.ECS.Compnents
{
    public class Interpolator : Component
    {
        public void Interpolate(IComponentContainer components, long delta)
        {
            var input = components.Get<DirectionalInput>();
            var velocity = components.Get<Velocity>();
            var transform = components.Get<Transform>();
            transform.MoveBy(velocity * input * (delta / Constants.MillisecondsInSecond));
        }
    }
}
