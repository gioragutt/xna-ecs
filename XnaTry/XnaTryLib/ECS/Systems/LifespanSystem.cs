using System;
using System.Collections.Generic;
using ECS.Interfaces;
using XnaCommonLib.ECS.Components;

namespace XnaCommonLib.ECS.Systems
{
    public class LifespanSystem : System
    {
        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            var initialCount = entities.Count;
            for (var i = 0; i < initialCount; ++i)
                entities[i].Get<Lifespan>().Update(delta);
        }

        public override Predicate<IComponentContainer> RelevantEntities() { return c => c.Has<Lifespan>(); }
    }
}
