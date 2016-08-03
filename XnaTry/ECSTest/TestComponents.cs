using ECS.Interfaces;

namespace ECSTest
{
    public class CounterComponent : IComponent
    {
        public IComponentContainer Container { get; set; }
        public bool Enabled { get; set; }
        public long Counter { get; set; }

        public CounterComponent() { Counter = 0; }
        public void Update(long delta) { Counter += delta; }
    }

    public class DummyComponent : IComponent
    {
        public IComponentContainer Container { get; set; }
        public bool Enabled { get; set; }
        public void Update(long delta) { }
    }

    public class AnotherDummyComponent : DummyComponent { }
}
