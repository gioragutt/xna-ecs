using ECS;

namespace ECSTest
{
    internal class CounterComponent : IComponent
    {
        public IComponentContainer Container { get; set; }
        public bool Enabled { get; set; }
        public long Counter { get; set; }

        public CounterComponent()
        {
            Counter = 0;
        }
        public void Update(long delta)
        {
            Counter += delta;
        }
    }

    internal class DummyComponent : IComponent
    {
        public IComponentContainer Container { get; set; }
        public bool Enabled { get; set; }

        public void Update(long delta)
        {
        }
    }
}
