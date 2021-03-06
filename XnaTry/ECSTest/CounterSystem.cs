﻿using System.Collections.Generic;
using System.Linq;
using ECS.Interfaces;

namespace ECSTest
{
    public class CounterSystem : ISystem
    {
        public bool Enabled { get; set; } = true;

        public void Update(IEntityPool pool, long delta)
        {
            Update(GetRelevant(pool), delta);
        }

        public IList<IComponentContainer> GetRelevant(IEntityPool pool)
        {
            return pool.AllThat(c => c.Has<CounterComponent>()).ToList();
        }

        public void Update(IList<IComponentContainer> entities, long delta)
        {
            foreach (var container in entities)
            {
                container.Get<CounterComponent>().Update(delta);
            }
        }
    }
}
