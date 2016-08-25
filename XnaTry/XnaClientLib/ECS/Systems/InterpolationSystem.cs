using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECS.Interfaces;
using XnaClientLib.ECS.Compnents;

namespace XnaClientLib.ECS.Systems
{
    public class InterpolationSystem : XnaCommonLib.ECS.Systems.System
    {
        public override void Update(IList<IComponentContainer> entities, long delta)
        {
            foreach (var entity in entities)
            {
                entity.Get<Interpolator>().Interpolate(entity, delta);
            }
        }

        public override Predicate<IComponentContainer> RelevantEntities() { return c => c.Has<Interpolator>(); }
    }
}
