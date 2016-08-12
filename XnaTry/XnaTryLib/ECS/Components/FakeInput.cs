using System;

namespace XnaTryLib.ECS.Components
{
    public class FakeInput : DirectionalInput
    {
        Random Random { get; } = new Random();
        private long timer = 0;
        private const long TimePerUpdate = 150;

        public override void Update(long delta)
        {
            if (!ShouldUpdate(delta))
                return;

            Horizontal = GetRandomInputValue();
            Vertical = GetRandomInputValue();
        }

        private bool ShouldUpdate(long delta)
        {
            timer += delta;
            if (timer < TimePerUpdate)
                return false;
            timer -= TimePerUpdate;
            return true;
        }

        private float GetRandomInputValue() { return (float)Random.NextDouble() * Random.Next(-1, 2); }
    }
}
