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
            Update(GetRelevant(pool), delta);   
        }

        public ICollection<CounterComponent> GetRelevant(IEntityPool pool)
        {
            return pool.GetAllOf<CounterComponent>();
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
