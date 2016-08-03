using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECSTest
{
    public class CounterSystem : ISystem<CounterComponent>
    {
        public bool Enabled { get; set; } = true;

        public void Update(IEntityPool pool, long delta)
        {
            var containers = GetRelevant(pool);
            var components = containers.Select(c => c.Get<CounterComponent>()).ToList();
            Update(components, delta);
        }

        public ICollection<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<CounterComponent>()).ToList();
        }

        public void Update(ICollection<CounterComponent> entities, long delta)
        {
            foreach (var comp in entities.Where(e => e.Enabled))
            {
                comp.Update(delta);
            }
        }
    }
}
